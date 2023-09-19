using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SG_ItemDragScript : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    // 아이템의 프리펩에 들어갈 스크립트

    private Transform canvasTrans;      // UI가 소속되어있는 최상단의 Canvas Transform
    private Transform previousParent;   // 해당 오브젝트가 직전에 소속되어 있던 부모의 Transform
    private RectTransform rectTrans;    // UI 위치 제어를 위한 RectTransform
    //private CanvasGroup canvasGroup;    // UI의 A 값과 상호 작용 제어를 위한 CanvasGroup

    // 23.09.12 10 : 48 위 선언문 수정하고 아래 동작 수정해야함
    private Image itemImage;

    private SG_ItemSlot thisParentSlotClass;
    private SG_ItemSlot giveItemSlotClass;       // 드래그 시작할떄 받아올 슬롯의 클래스
    private SG_ItemSlot acceptItemSlotClass;     // 드롭 할때에 받아올 클래스
    private SG_ItemSwapManager swapManagerClass; // 스왑 처리를 위한 클래스
    
    private SG_ItemDragScript thisClass;  

    // 임시 주는 슬롯 받는 슬롯 고유번호 여기서 받아서 넘겨줄 거임
    private int giveSlotCount;
    private int acceptSlotCount;

    [SerializeField]
    private LayerMask slotLayer;

    public delegate void RayTargerEventHandler();
    public event RayTargerEventHandler RayTargerEvent;

    // 마우스 포인터 이벤트 데이터 생성
    PointerEventData pointerEventDataDown;
    List<RaycastResult> resultsDown;// List로 변경

    // 마우스 포인터 이벤트 데이터 생성   
    PointerEventData pointerEventDataUp;
    List<RaycastResult> resultsUp; // List로 변경

    // Test
    public static List<SG_ItemDragScript> allItemDragScrip = default;

    private void Awake()
    {
        canvasTrans = FindAnyObjectByType<Canvas>().transform;
        swapManagerClass = FindAnyObjectByType<SG_ItemSwapManager>();

        rectTrans = GetComponent<RectTransform>();
        itemImage = GetComponent<Image>();
        //canvasGroup = GetComponent<CanvasGroup>();

        resultsUp = new List<RaycastResult>();
        resultsDown = new List<RaycastResult>();
        pointerEventDataDown = new PointerEventData(EventSystem.current);
        pointerEventDataUp = new PointerEventData(EventSystem.current);

        // Init Test
        if(allItemDragScrip == null || allItemDragScrip == default)
        {
            allItemDragScrip = new List<SG_ItemDragScript>();
        }
        allItemDragScrip.Add(this);
    }

    public void Start()
    {
        RayTargerEvent += RayTargetControler_All;

        thisClass = this.GetComponent<SG_ItemDragScript>();
    }

    /// <summary>
    /// 현재 오브젝트를 드래그하기 시작할 때 1회 호출
    /// </summary>    
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그 직전에 소속되어 있던 부모 Transform 정보 저장
        previousParent = this.transform.parent;

        // 현재 드래그중인 UI가 화면의 최상단에 출력되도록 하기 위해
        this.transform.SetParent(canvasTrans);  // 부모 오브젝트를 Canvas로 설정
        transform.SetAsLastSibling();           // 가장 앞에 보이도록 마지막 자식으로 설정        

        // 클릭한 슬롯의 고유번호 추출을 위한 함수
        ClickDown();
        RayTargerEvent?.Invoke();
        //itemImage.raycastTarget = false;    //드래그할때에 레이가 끌고있는것에 맞지않도록 Target flase

    }

    /// <summary>
    /// 현재 오브젝트를 드래그 중일 때 매 프레임 호출
    /// </summary>    
    public void OnDrag(PointerEventData eventData)
    {
        // 현재 스크린상의 마우스 위치를 UI 위치로 설정 (UI가 마우스를 쫒아다니는 상태)
        rectTrans.position = eventData.position;

    }

    /// <summary>
    /// 현재 오브젝트의 드래그를 종료할 때 1회 호출
    /// </summary>    
    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그를 시작하면 부모가 Canvas로 설정되기 떄문에
        // 드래그를 종료할 때 부모가 Canvas이면 아이템 슬롯이 아닌 엉뚱한 곳에
        // 드롭을 했다는 뜻이기 때문에 드래그 직전에 소속되어 있던 아이템 슬롯으로 아이템 이동
        if (this.transform.parent == canvasTrans)
        {
            // 마지막에 소속되어 잇었던 previousParent의 자식으로 설정하고, 해당 위치로 설정
            transform.SetParent(previousParent);
            rectTrans.position = previousParent.GetComponent<RectTransform>().position;
        }
        //canvasGroup.blocksRaycasts = true;

        // 자신의 상위 오브젝트의 태그 = Slot 의 태그에 따라서 주는인벤토리가 어느곳인지 판단할함수        
        // 받는곳의 Slot이 어떤건지 UI로 체크 해주는 함수        
        ClickUp();
        RayTargerEvent?.Invoke();

        //itemImage.raycastTarget = true;

    }

    public void OnDrop(PointerEventData eventData)
    {

    }


    // 마우스 포인터가 UI어떤것을 찍었는지 체크해주는 함수
    // 추후 ItemSwap보낼때 받을 친구로 사용할거임
    // 자신의 부모오브젝트에 태그가 무엇인지에 따라 주는 오브젝트가 달라짐
    private void ClickUp()
    {
        pointerEventDataUp.position = Input.mousePosition;

        EventSystem.current.RaycastAll(pointerEventDataUp, resultsUp);

        if (resultsUp.Count > 0)
        {
            // UI 요소를 클릭했을 때의 처리를 여기에 추가
            GameObject clickedUIElement = resultsUp[0].gameObject;

            // 여기에 감지된 clikedUIElement 변수에서 누른 슬롯을 추출해내면 될거같음
            //Debug.Log("클릭한 UI 요소: " + clickedUIElement.tag);

            //여기서 고유번호 추출
            if (clickedUIElement.CompareTag("ItemSlot")) // 아이템 슬롯을 눌렀을경우
            {
                acceptItemSlotClass = clickedUIElement.GetComponent<SG_ItemSlot>();
                //누른 아이템슬롯의 고유번호 추출
                acceptSlotCount = acceptItemSlotClass.slotCount;
                //Debug.LogFormat("클릭을 땔때주는 스롯의 고유번호 -> {0}", acceptSlotCount);
                //Debug.LogFormat("받는 스롯의 고유번호 -> {0}",acceptSlotCount);

                // 클릭한 UI 요소에 대한 작업을 수행할 수 있습니다.
                //Debug.LogFormat("받는얘 번호 -> {0}", acceptSlotCount);
                // 아래함수 테스트후 아래함수는 giveSlotCount != null && acceptSlotCount != null 로 조건넣으면 될듯
                //Debug.LogFormat("giveSlotCount -> {0} acceptSlotCount -> {1}", giveSlotCount, acceptSlotCount);
                //Debug.LogFormat("giveItemSlotClass = null? -> {0}  acceptItemSlotClass = null? -> {1}", giveItemSlotClass == null, acceptItemSlotClass == null);
                //Debug.LogFormat("giveItemSlotClass -> {0}  acceptItemSlotClass -> {1}", giveItemSlotClass, acceptItemSlotClass);
                //Debug.Log(giveItemSlotClass.transform.parent.parent == gameObject);
                //Debug.LogFormat("Swap 매개변수 null 인지 thisClass -> {0}, giveSlotCount -> {1}, acceptSlotCount -> {2}" +
                //    "giveItemSlot == null? -> {3}, acceptItemSlotClass == null? -> {4} ", thisClass == null,giveSlotCount,acceptSlotCount
                //    ,giveItemSlotClass == null,acceptItemSlotClass == null);
                    swapManagerClass.ItemSwap(thisClass,giveSlotCount, acceptSlotCount, giveItemSlotClass, acceptItemSlotClass);

                if (giveItemSlotClass != acceptItemSlotClass)
                {

                }
            }
        }
        else
        {
            // UI 요소가 없거나 클릭하지 않았을 때의 처리를 여기에 추가
            //Debug.Log("Click Up UI 요소를 클릭하지 않았거나, 클릭한 UI 요소가 없습니다.");
        }

    }   // ClickUP()

    // 클릭했을때에 UI라면 해당하는 슬롯의 고유번호를 받아줄 함수
    private void ClickDown()
    {
        //Debug.Log("ClickDown은 들어오긴하나?" +"");
        // 마우스 포인터 이벤트 데이터 생성       
        pointerEventDataDown.position = Input.mousePosition;

        EventSystem.current.RaycastAll(pointerEventDataDown, resultsDown);

        if (resultsDown.Count > 0)
        {
            // UI 요소를 클릭했을 때의 처리를 여기에 추가
            GameObject clickedUIElement = resultsDown[0].gameObject;

            // 여기에 감지된 clikedUIElement 변수에서 누른 슬롯을 추출해내면 될거같음
            //Debug.Log("클릭한 UI 요소: " + clickedUIElement.tag);

            Debug.Log(clickedUIElement.tag);
            if (clickedUIElement.CompareTag("ItemSlot")) // 아이템 슬롯을 눌렀을경우
            {
                //Debug.Log("ClickDown속 Tag 조건에 들어오나?");

                giveItemSlotClass = clickedUIElement.GetComponent<SG_ItemSlot>();
                //누른 아이템슬롯의 고유번호 추출
                giveSlotCount = giveItemSlotClass.slotCount;
                //Debug.LogFormat("주는얘 번호 -> {0}", giveSlotCount);
                //Debug.LogFormat("클릭시 스롯의 고유번호 -> {0}", giveSlotCount);
            }

        }
        else
        {
            // UI 요소가 없거나 클릭하지 않았을 때의 처리를 여기에 추가
            //Debug.Log("ClickIn() UI 요소를 클릭하지 않았거나, 클릭한 UI 요소가 없습니다.");
        }
    }


    protected void RayTargetControler()
    {
        //Debug.Log("이벤트 호출");
        if (itemImage.raycastTarget == true)
        {
            itemImage.raycastTarget = false;
        }

        else if (itemImage.raycastTarget == false)
        {
            itemImage.raycastTarget = true;
        }
    }       // RayTargetControler()

    public void RayTargetControler_All()
    {
        foreach(var ele in allItemDragScrip)
        {
            ele.RayTargetControler();
        }
    }       // RayTargetControler()


    // 스왑후 잔여아이템이 남아있다면 다시 부모 오브젝트로가게 만들 함수
    public void SetTransformParent()
    {
        //Debug.Log("원래대로 돌려주며 Sprite 를 집어넣는 함수에 잘들어오나?");
        // 마지막에 소속되어 잇었던 previousParent의 자식으로 설정하고, 해당 위치로 설정
        transform.SetParent(previousParent);
        rectTrans.position = previousParent.GetComponent<RectTransform>().position;
      
        // 다시 돌아왔으니 Item InIt 해줘야할듯
        thisParentSlotClass = this.transform.parent.gameObject.GetComponent<SG_ItemSlot>();
        thisParentSlotClass.MoveItemSet();

    }

}   // NAMESPACE

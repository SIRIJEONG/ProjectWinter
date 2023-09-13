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

    // 23.09.13 Drag 쪽에서 쏴줘야 할거같음

    private SG_ItemSlot playerItemSlotClass;
    private SG_WareHouseItemSlot werehouseItemSlotClass;
    private SG_ItemSwapManager swapManagerClass;

    // 아래부터 자신과 상대를 검출하기 위한 Class 선언
    private SG_ItemSlot giveByPlayer;
    private SG_WareHouseItemSlot giveByWareHouse;
    //perent = this.transform.parent.gameObject.GetComponent<SG_ItemSlot>();

    // 임시 주는 슬롯 받는 슬롯 고유번호 여기서 받아서 넘겨줄 거임
    private int giveSlotCount;
    private int acceptSlotCount;





    private void Awake()
    {
        canvasTrans = FindAnyObjectByType<Canvas>().transform;
        rectTrans = GetComponent<RectTransform>();

        //canvasGroup = GetComponent<CanvasGroup>();
        itemImage = GetComponent<Image>();

       
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

        // 드래그 가능한 오브젝트가 하나가 아닌 자식들을 가지고 있을 수도 있기 때문에 CanvasGroup으로
        // 알파값을 0.6으로 설정하고, 광선 충돌처리가 되지 않도록 한다.
        //canvasGroup.alpha = 0.6f;
        //canvasGroup.blocksRaycasts = false;

        // 클릭한 슬롯의 고유번호 추출을 위한 함수
        ClickDown();
        itemImage.raycastTarget = false;

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


        //TODO : 아래는 매개변수가 플레이어가 창고로 옮기는것인데 추후 따로두던지 차이점을 주어서 누가 주었는지 인식시켜야
        // 다방면으올 사용가능할거같음
        // 위 문제 방안 1 -> 자신의 부모오브젝트의 tag에 따라서 주는스크립트와 받는 스크립트를 구별한다
        //  클래스의 선언이 좀 많아질것으로 예상


        itemImage.raycastTarget = true;
    }

    public void OnDrop(PointerEventData eventData)
    {

    }


    // 마우스 포인터가 UI어떤것을 찍었는지 체크해주는 함수
    // 추후 ItemSwap보낼때 받을 친구로 사용할거임
    // 자신의 부모오브젝트에 태그가 무엇인지에 따라 주는 오브젝트가 달라짐
    private void ClickUp()
    {
        // 마우스 포인터 이벤트 데이터 생성
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        // UI 요소 검출
        List<RaycastResult> results = new List<RaycastResult>(); // List로 변경
        EventSystem.current.RaycastAll(pointerEventData, results);

        if (results.Count > 0)
        {
            // UI 요소를 클릭했을 때의 처리를 여기에 추가
            GameObject clickedUIElement = results[0].gameObject;

            // 여기에 감지된 clikedUIElement 변수에서 누른 슬롯을 추출해내면 될거같음
            Debug.Log("클릭한 UI 요소: " + clickedUIElement.tag);
          
            //여기서 고유번호 추출
            if(clickedUIElement.gameObject.CompareTag("WareHouseSlot"))
            {
                werehouseItemSlotClass = clickedUIElement.GetComponent<SG_WareHouseItemSlot>();
                acceptSlotCount = werehouseItemSlotClass.wareHouseSlotCount;
            }

            // 클릭한 UI 요소에 대한 작업을 수행할 수 있습니다.
        }
        else
        {
            // UI 요소가 없거나 클릭하지 않았을 때의 처리를 여기에 추가
            Debug.Log("Click Up UI 요소를 클릭하지 않았거나, 클릭한 UI 요소가 없습니다.");
        }

        //Debug.Log(giveByPlayer == null);


        if (this.transform.parent.gameObject.CompareTag("PlayerSlot"))
        {
            giveByPlayer = this.transform.parent.gameObject.GetComponent<SG_ItemSlot>();
        }
        else if(this.transform.parent.gameObject.CompareTag("WareHouseSlot"))
        {
            giveByWareHouse = this.transform.parent.gameObject.GetComponent<SG_WareHouseItemSlot>();
        }

        

        // 이곳에서 자신이 누구인지 눌렀을때 대상이 누구인지 알수 있음
        // 여기서 Swap을 쏴주면 될거같기도함
        if(giveByPlayer != null) //눌럿다 땟는데 슬롯이 플레이어의 것이면
        {
            // 매개변수 정보 : 주는얘 주소, 받는얘주소 , 플레이어 슬롯, 인벤토리 슬롯
            swapManagerClass.ItemSwap(giveSlotCount, acceptSlotCount, playerItemSlotClass, werehouseItemSlotClass);
            giveByPlayer = null;
        }



    }   // ClickUP()

    // 클릭했을때에 UI라면 해당하는 슬롯의 고유번호를 받아줄 함수
    private void ClickDown()
    {
        // 마우스 포인터 이벤트 데이터 생성
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        // UI 요소 검출
        List<RaycastResult> results = new List<RaycastResult>(); // List로 변경
        EventSystem.current.RaycastAll(pointerEventData, results);

        if (results.Count > 0)
        {
            // UI 요소를 클릭했을 때의 처리를 여기에 추가
            GameObject clickedUIElement = results[0].gameObject;

            // 여기에 감지된 clikedUIElement 변수에서 누른 슬롯을 추출해내면 될거같음
            Debug.Log("클릭한 UI 요소: " + clickedUIElement.name);


            if(clickedUIElement.CompareTag("PlayerSlot")) // 플레이어 슬롯을 눌렀을경우
            {
                playerItemSlotClass = clickedUIElement.GetComponent<SG_ItemSlot>();
                //누른플레이어의 고유번호 추출
                giveSlotCount = playerItemSlotClass.slotCount;
            }

            

            // 클릭한 UI 요소에 대한 작업을 수행할 수 있습니다.
        }
        else
        {
            // UI 요소가 없거나 클릭하지 않았을 때의 처리를 여기에 추가
            Debug.Log("ClickIn() UI 요소를 클릭하지 않았거나, 클릭한 UI 요소가 없습니다.");
        }
    }





}   // NAMESPACE

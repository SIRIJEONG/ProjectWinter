using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SG_ItemDragScript : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    // �������� �����鿡 �� ��ũ��Ʈ

    private Transform canvasTrans;      // UI�� �ҼӵǾ��ִ� �ֻ���� Canvas Transform
    private Transform previousParent;   // �ش� ������Ʈ�� ������ �ҼӵǾ� �ִ� �θ��� Transform
    private RectTransform rectTrans;    // UI ��ġ ��� ���� RectTransform
    //private CanvasGroup canvasGroup;    // UI�� A ���� ��ȣ �ۿ� ��� ���� CanvasGroup

    // 23.09.12 10 : 48 �� ���� �����ϰ� �Ʒ� ���� �����ؾ���
    private Image itemImage;

    private SG_ItemSlot thisParentSlotClass;
    private SG_ItemSlot giveItemSlotClass;       // �巡�� �����ҋ� �޾ƿ� ������ Ŭ����
    private SG_ItemSlot acceptItemSlotClass;     // ��� �Ҷ��� �޾ƿ� Ŭ����
    private SG_ItemSwapManager swapManagerClass; // ���� ó���� ���� Ŭ����
    
    private SG_ItemDragScript thisClass;  

    // �ӽ� �ִ� ���� �޴� ���� ������ȣ ���⼭ �޾Ƽ� �Ѱ��� ����
    private int giveSlotCount;
    private int acceptSlotCount;

    [SerializeField]
    private LayerMask slotLayer;

    public delegate void RayTargerEventHandler();
    public event RayTargerEventHandler RayTargerEvent;

    // ���콺 ������ �̺�Ʈ ������ ����
    PointerEventData pointerEventDataDown;
    List<RaycastResult> resultsDown;// List�� ����

    // ���콺 ������ �̺�Ʈ ������ ����   
    PointerEventData pointerEventDataUp;
    List<RaycastResult> resultsUp; // List�� ����

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
    /// ���� ������Ʈ�� �巡���ϱ� ������ �� 1ȸ ȣ��
    /// </summary>    
    public void OnBeginDrag(PointerEventData eventData)
    {
        // �巡�� ������ �ҼӵǾ� �ִ� �θ� Transform ���� ����
        previousParent = this.transform.parent;

        // ���� �巡������ UI�� ȭ���� �ֻ�ܿ� ��µǵ��� �ϱ� ����
        this.transform.SetParent(canvasTrans);  // �θ� ������Ʈ�� Canvas�� ����
        transform.SetAsLastSibling();           // ���� �տ� ���̵��� ������ �ڽ����� ����        

        // Ŭ���� ������ ������ȣ ������ ���� �Լ�
        ClickDown();
        RayTargerEvent?.Invoke();
        //itemImage.raycastTarget = false;    //�巡���Ҷ��� ���̰� �����ִ°Ϳ� �����ʵ��� Target flase

    }

    /// <summary>
    /// ���� ������Ʈ�� �巡�� ���� �� �� ������ ȣ��
    /// </summary>    
    public void OnDrag(PointerEventData eventData)
    {
        // ���� ��ũ������ ���콺 ��ġ�� UI ��ġ�� ���� (UI�� ���콺�� �i�ƴٴϴ� ����)
        rectTrans.position = eventData.position;

    }

    /// <summary>
    /// ���� ������Ʈ�� �巡�׸� ������ �� 1ȸ ȣ��
    /// </summary>    
    public void OnEndDrag(PointerEventData eventData)
    {
        // �巡�׸� �����ϸ� �θ� Canvas�� �����Ǳ� ������
        // �巡�׸� ������ �� �θ� Canvas�̸� ������ ������ �ƴ� ������ ����
        // ����� �ߴٴ� ���̱� ������ �巡�� ������ �ҼӵǾ� �ִ� ������ �������� ������ �̵�
        if (this.transform.parent == canvasTrans)
        {
            // �������� �ҼӵǾ� �վ��� previousParent�� �ڽ����� �����ϰ�, �ش� ��ġ�� ����
            transform.SetParent(previousParent);
            rectTrans.position = previousParent.GetComponent<RectTransform>().position;
        }
        //canvasGroup.blocksRaycasts = true;

        // �ڽ��� ���� ������Ʈ�� �±� = Slot �� �±׿� ���� �ִ��κ��丮�� ��������� �Ǵ����Լ�        
        // �޴°��� Slot�� ����� UI�� üũ ���ִ� �Լ�        
        ClickUp();
        RayTargerEvent?.Invoke();

        //itemImage.raycastTarget = true;

    }

    public void OnDrop(PointerEventData eventData)
    {

    }


    // ���콺 �����Ͱ� UI����� ������� üũ���ִ� �Լ�
    // ���� ItemSwap������ ���� ģ���� ����Ұ���
    // �ڽ��� �θ������Ʈ�� �±װ� ���������� ���� �ִ� ������Ʈ�� �޶���
    private void ClickUp()
    {
        pointerEventDataUp.position = Input.mousePosition;

        EventSystem.current.RaycastAll(pointerEventDataUp, resultsUp);

        if (resultsUp.Count > 0)
        {
            // UI ��Ҹ� Ŭ������ ���� ó���� ���⿡ �߰�
            GameObject clickedUIElement = resultsUp[0].gameObject;

            // ���⿡ ������ clikedUIElement �������� ���� ������ �����س��� �ɰŰ���
            //Debug.Log("Ŭ���� UI ���: " + clickedUIElement.tag);

            //���⼭ ������ȣ ����
            if (clickedUIElement.CompareTag("ItemSlot")) // ������ ������ ���������
            {
                acceptItemSlotClass = clickedUIElement.GetComponent<SG_ItemSlot>();
                //���� �����۽����� ������ȣ ����
                acceptSlotCount = acceptItemSlotClass.slotCount;
                //Debug.LogFormat("Ŭ���� �����ִ� ������ ������ȣ -> {0}", acceptSlotCount);
                //Debug.LogFormat("�޴� ������ ������ȣ -> {0}",acceptSlotCount);

                // Ŭ���� UI ��ҿ� ���� �۾��� ������ �� �ֽ��ϴ�.
                //Debug.LogFormat("�޴¾� ��ȣ -> {0}", acceptSlotCount);
                // �Ʒ��Լ� �׽�Ʈ�� �Ʒ��Լ��� giveSlotCount != null && acceptSlotCount != null �� ���ǳ����� �ɵ�
                //Debug.LogFormat("giveSlotCount -> {0} acceptSlotCount -> {1}", giveSlotCount, acceptSlotCount);
                //Debug.LogFormat("giveItemSlotClass = null? -> {0}  acceptItemSlotClass = null? -> {1}", giveItemSlotClass == null, acceptItemSlotClass == null);
                //Debug.LogFormat("giveItemSlotClass -> {0}  acceptItemSlotClass -> {1}", giveItemSlotClass, acceptItemSlotClass);
                //Debug.Log(giveItemSlotClass.transform.parent.parent == gameObject);
                //Debug.LogFormat("Swap �Ű����� null ���� thisClass -> {0}, giveSlotCount -> {1}, acceptSlotCount -> {2}" +
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
            // UI ��Ұ� ���ų� Ŭ������ �ʾ��� ���� ó���� ���⿡ �߰�
            //Debug.Log("Click Up UI ��Ҹ� Ŭ������ �ʾҰų�, Ŭ���� UI ��Ұ� �����ϴ�.");
        }

    }   // ClickUP()

    // Ŭ���������� UI��� �ش��ϴ� ������ ������ȣ�� �޾��� �Լ�
    private void ClickDown()
    {
        //Debug.Log("ClickDown�� �������ϳ�?" +"");
        // ���콺 ������ �̺�Ʈ ������ ����       
        pointerEventDataDown.position = Input.mousePosition;

        EventSystem.current.RaycastAll(pointerEventDataDown, resultsDown);

        if (resultsDown.Count > 0)
        {
            // UI ��Ҹ� Ŭ������ ���� ó���� ���⿡ �߰�
            GameObject clickedUIElement = resultsDown[0].gameObject;

            // ���⿡ ������ clikedUIElement �������� ���� ������ �����س��� �ɰŰ���
            //Debug.Log("Ŭ���� UI ���: " + clickedUIElement.tag);

            Debug.Log(clickedUIElement.tag);
            if (clickedUIElement.CompareTag("ItemSlot")) // ������ ������ ���������
            {
                //Debug.Log("ClickDown�� Tag ���ǿ� ������?");

                giveItemSlotClass = clickedUIElement.GetComponent<SG_ItemSlot>();
                //���� �����۽����� ������ȣ ����
                giveSlotCount = giveItemSlotClass.slotCount;
                //Debug.LogFormat("�ִ¾� ��ȣ -> {0}", giveSlotCount);
                //Debug.LogFormat("Ŭ���� ������ ������ȣ -> {0}", giveSlotCount);
            }

        }
        else
        {
            // UI ��Ұ� ���ų� Ŭ������ �ʾ��� ���� ó���� ���⿡ �߰�
            //Debug.Log("ClickIn() UI ��Ҹ� Ŭ������ �ʾҰų�, Ŭ���� UI ��Ұ� �����ϴ�.");
        }
    }


    protected void RayTargetControler()
    {
        //Debug.Log("�̺�Ʈ ȣ��");
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


    // ������ �ܿ��������� �����ִٸ� �ٽ� �θ� ������Ʈ�ΰ��� ���� �Լ�
    public void SetTransformParent()
    {
        //Debug.Log("������� �����ָ� Sprite �� ����ִ� �Լ��� �ߵ�����?");
        // �������� �ҼӵǾ� �վ��� previousParent�� �ڽ����� �����ϰ�, �ش� ��ġ�� ����
        transform.SetParent(previousParent);
        rectTrans.position = previousParent.GetComponent<RectTransform>().position;
      
        // �ٽ� ���ƿ����� Item InIt ������ҵ�
        thisParentSlotClass = this.transform.parent.gameObject.GetComponent<SG_ItemSlot>();
        thisParentSlotClass.MoveItemSet();

    }

}   // NAMESPACE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;

public class SG_ItemDragScript : MonoBehaviourPun,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    // �������� �����鿡 �� ��ũ��Ʈ

    private Transform canvasTrans;      // UI�� �ҼӵǾ��ִ� �ֻ���� Canvas Transform
    private Transform previousParent;   // �ش� ������Ʈ�� ������ �ҼӵǾ� �ִ� �θ��� Transform
    private RectTransform rectTrans;    // UI ��ġ ��� ���� RectTransform  

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

    private Transform topParentTrans;

    // �︮�е峪 �����⸦ �巡�� �õ��������� �̰��� true�� �Ǿ end Drag���� �Լ�ȣ�� �Ѱ��ٰ���
    private bool isPassTargetControll = false;

    // �巡���Ҷ��� ������ �̹������� RayTarger�� ���� List
    public static List<SG_ItemDragScript> allItemDragScrip = default;

    private void Awake()
    {
        FirstInIt();            // ó�� ������ GetComponent���� �Լ�
    }

    public void Start()
    {
        StartInIt();            // ó�� ������ GetComponent���� �Լ� 
    }

    /// <summary>
    /// ���� ������Ʈ�� �巡���ϱ� ������ �� 1ȸ ȣ��
    /// </summary>    
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.LogFormat("�巡���� top �θ� -> {0}", topParentTrans.tag);
        if (topParentTrans.CompareTag("PowerStation") || topParentTrans.CompareTag("HeliPad"))
        {
            isPassTargetControll = true;
            return;
        }
        // �巡�� ������ �ҼӵǾ� �ִ� �θ� Transform ���� ����
        previousParent = this.transform.parent;
        //Debug.LogFormat("PrebiousParentName -> {0}     PrebiousParentTag -> {1}", previousParent.name, previousParent.tag);
        // ���� �巡������ UI�� ȭ���� �ֻ�ܿ� ��µǵ��� �ϱ� ����
        Debug.LogFormat("canvasTransName == null? -> {0}", canvasTrans == null);
        this.transform.SetParent(canvasTrans);  // �θ� ������Ʈ�� Canvas�� ����
        transform.SetAsFirstSibling();
        //transform.SetAsLastSibling();           // ���� �տ� ���̵��� ������ �ڽ����� ����

        RayTargerEvent?.Invoke();
        // Ŭ���� ������ ������ȣ ������ ���� �Լ�
        ClickDown();

    }

    /// <summary>
    /// ���� ������Ʈ�� �巡�� ���� �� �� ������ ȣ��
    /// </summary>    
    public void OnDrag(PointerEventData eventData)
    {

        //Debug.LogFormat("topParentTrans.name -> {0}", topParentTrans.name);
        //Debug.LogFormat("topParentTrans.tag -> {0}", topParentTrans.tag);

        // ���� ��ũ������ ���콺 ��ġ�� UI ��ġ�� ���� (UI�� ���콺�� �i�ƴٴϴ� ����)
        if (topParentTrans.CompareTag("PowerStation") || topParentTrans.CompareTag("HeliPad"))
        {
            return;
        }
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

        if (isPassTargetControll == true)
        {
            Debug.Log("�︮�е� Ȥ�� �������� �������� �ǵ�� PASS �մϴ�.");
            isPassTargetControll = false;
            return;
        }

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
        //Debug.Log("ClickUP�� ������?");    
        if (resultsUp.Count > 0)
        {
            // UI ��Ҹ� Ŭ������ ���� ó���� ���⿡ �߰�
            GameObject clickedUIElement = resultsUp[0].gameObject;

            //Debug.Log("Ŭ���� UI ���: " + clickedUIElement.tag);

            //���⼭ ������ȣ ����
            if (clickedUIElement.CompareTag("ItemSlot")) // ������ ������ ���������
            {
                acceptItemSlotClass = clickedUIElement.GetComponent<SG_ItemSlot>();
                //���� �����۽����� ������ȣ ����
                acceptSlotCount = acceptItemSlotClass.slotCount;

                #region DEBUG
                //Debug.LogFormat("Ŭ���� �����ִ� ������ ������ȣ -> {0}", acceptSlotCount);                
                //Debug.LogFormat("�޴¾� ��ȣ -> {0}", acceptSlotCount);
                // �Ʒ��Լ� �׽�Ʈ�� �Ʒ��Լ��� giveSlotCount != null && acceptSlotCount != null �� ���ǳ����� �ɵ�
                //Debug.LogFormat("giveSlotCount -> {0} acceptSlotCount -> {1}", giveSlotCount, acceptSlotCount);
                //Debug.LogFormat("giveItemSlotClass = null? -> {0}  acceptItemSlotClass = null? -> {1}", giveItemSlotClass == null, acceptItemSlotClass == null);
                //Debug.LogFormat("giveItemSlotClass -> {0}  acceptItemSlotClass -> {1}", giveItemSlotClass, acceptItemSlotClass);
                //Debug.Log(giveItemSlotClass.transform.parent.parent == gameObject);
                //Debug.LogFormat("Swap �Ű����� null ���� thisClass -> {0}, giveSlotCount -> {1}, acceptSlotCount -> {2}" +
                //    "giveItemSlot == null? -> {3}, acceptItemSlotClass == null? -> {4} ", thisClass == null,giveSlotCount,acceptSlotCount
                //    ,giveItemSlotClass == null,acceptItemSlotClass == null);
                #endregion DEBUG
                //�Ʒ� ItemSwap �Լ��� Mastrt�� �ҷ���� �Ҽ��� ����
                swapManagerClass.ItemSwap(thisClass, giveSlotCount, acceptSlotCount, giveItemSlotClass, acceptItemSlotClass);

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

            //Debug.LogFormat("ClickDown Tag -> {0}   Name -> {1}", clickedUIElement.tag, clickedUIElement.name);
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
        foreach (var ele in allItemDragScrip)
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

    private void FirstInIt()    // Awake�κп��� ������ ���Ե� �͵�
    {

        swapManagerClass = FindAnyObjectByType<SG_ItemSwapManager>();

        rectTrans = GetComponent<RectTransform>();
        itemImage = GetComponent<Image>();
        //canvasGroup = GetComponent<CanvasGroup>();

        resultsUp = new List<RaycastResult>();
        resultsDown = new List<RaycastResult>();
        pointerEventDataDown = new PointerEventData(EventSystem.current);
        pointerEventDataUp = new PointerEventData(EventSystem.current);

        // Init Test
        if (allItemDragScrip == null || allItemDragScrip == default)    // ���� Static���� �ö� allItemDragScript�� ��������� �Ҵ�
        {
            allItemDragScrip = new List<SG_ItemDragScript>();
        }
        allItemDragScrip.Add(this);
    }

    private void StartInIt()    //Start �ܰ迡�� ������ ���Եɰ͵�
    {
        RayTargerEvent += RayTargetControler_All;

        thisClass = this.GetComponent<SG_ItemDragScript>();

        SerchTopParentObj();
        FindCanvasTransform(topParentTrans);
    }

    public void CanvasUpdate()  // ������ �̵��� �����Ǿ��� Canvas�� �ֽ�ȭ ����
    {
        //Debug.LogFormat("������ ������� �ߺҷ�����? ");
        SerchTopParentObj();
        FindCanvasTransform(topParentTrans);
    }



    // topParentTrans ������Ʈ�� �ڽ� �߿��� Canvas�� transform�� ã���ϴ�
    private void FindCanvasTransform(Transform parent)
    {
        foreach (Transform child in parent)
        {
            // Canvas ������Ʈ�� ã���� ��� �ش� transform�� ��ȯ�մϴ�
            Canvas canvas = child.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvasTrans = canvas.transform;
                //Debug.LogFormat("What is this? -> {0}", canvasTrans);
                return;
            }
        }
        // Canvas�� ã�� ���� ��� null�� ��ȯ�մϴ�
        Debug.Log("canvasTrans == null");
        return;
    }

    private void SerchTopParentObj()
    {
        topParentTrans = this.transform.parent;

        //�ֻ��� �θ� ������Ʈ �±׸� �������� ���� ã�� ����
        while (topParentTrans.transform.parent != null)
        {
            topParentTrans = topParentTrans.transform.parent;
        }
    }

}   // NAMESPACE

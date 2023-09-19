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
     
    private SG_ItemSlot giveItemSlotClass;       // �巡�� �����ҋ� �޾ƿ� ������ Ŭ����
    private SG_ItemSlot acceptItemSlotClass;     // ��� �Ҷ��� �޾ƿ� Ŭ����
    private SG_ItemSwapManager swapManagerClass; // ���� ó���� ���� Ŭ����

    // �ӽ� �ִ� ���� �޴� ���� ������ȣ ���⼭ �޾Ƽ� �Ѱ��� ����
    private int giveSlotCount;
    private int acceptSlotCount;

    [SerializeField]
    private LayerMask slotLayer;


    private void Awake()
    {  
        canvasTrans = FindAnyObjectByType<Canvas>().transform;       
        swapManagerClass = FindAnyObjectByType<SG_ItemSwapManager>();  

        rectTrans = GetComponent<RectTransform>();
        itemImage = GetComponent<Image>();
        //canvasGroup = GetComponent<CanvasGroup>();
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
        itemImage.raycastTarget = false;    //�巡���Ҷ��� ���̰� �����ִ°Ϳ� �����ʵ��� Target flase

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

        


        itemImage.raycastTarget = true;
    }

    public void OnDrop(PointerEventData eventData)
    {

    }


    // ���콺 �����Ͱ� UI����� ������� üũ���ִ� �Լ�
    // ���� ItemSwap������ ���� ģ���� ����Ұ���
    // �ڽ��� �θ������Ʈ�� �±װ� ���������� ���� �ִ� ������Ʈ�� �޶���
    private void ClickUp()
    {
        // ���콺 ������ �̺�Ʈ ������ ����
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        // UI ��� ����
        List<RaycastResult> results = new List<RaycastResult>(); // List�� ����
        EventSystem.current.RaycastAll(pointerEventData, results);

        if (results.Count > 0)
        {
            // UI ��Ҹ� Ŭ������ ���� ó���� ���⿡ �߰�
            GameObject clickedUIElement = results[0].gameObject;

            // ���⿡ ������ clikedUIElement �������� ���� ������ �����س��� �ɰŰ���
            Debug.Log("Ŭ���� UI ���: " + clickedUIElement.tag);

            //���⼭ ������ȣ ����
            if (clickedUIElement.CompareTag("ItemSlot")) // ������ ������ ���������
            {
                acceptItemSlotClass = clickedUIElement.GetComponent<SG_ItemSlot>();
                //���� �����۽����� ������ȣ ����
                acceptSlotCount = acceptItemSlotClass.slotCount;
                Debug.LogFormat("Ŭ���� �����ִ� ������ ������ȣ -> {0}", acceptSlotCount);
                //Debug.LogFormat("�޴� ������ ������ȣ -> {0}",acceptSlotCount);
            }

            // Ŭ���� UI ��ҿ� ���� �۾��� ������ �� �ֽ��ϴ�.

            // �Ʒ��Լ� �׽�Ʈ�� �Ʒ��Լ��� giveSlotCount != null && acceptSlotCount != null �� ���ǳ����� �ɵ�
            Debug.LogFormat("giveSlotCount -> {0} acceptSlotCount -> {1}", giveSlotCount, acceptSlotCount);
            Debug.LogFormat("giveItemSlotClass = null? -> {0}  acceptItemSlotClass = null? -> {1}", giveItemSlotClass == null, acceptItemSlotClass == null);
            Debug.LogFormat("giveItemSlotClass -> {0}  acceptItemSlotClass -> {1}", giveItemSlotClass, acceptItemSlotClass);
            Debug.Log(giveItemSlotClass.transform.parent.parent == gameObject);
            swapManagerClass.ItemSwap(giveSlotCount, acceptSlotCount, giveItemSlotClass, acceptItemSlotClass);
        }
        else
        {
            // UI ��Ұ� ���ų� Ŭ������ �ʾ��� ���� ó���� ���⿡ �߰�
            Debug.Log("Click Up UI ��Ҹ� Ŭ������ �ʾҰų�, Ŭ���� UI ��Ұ� �����ϴ�.");
        }


  
    }   // ClickUP()

    // Ŭ���������� UI��� �ش��ϴ� ������ ������ȣ�� �޾��� �Լ�
    private void ClickDown()
    {
        // ���콺 ������ �̺�Ʈ ������ ����
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        // UI ��� ����
        List<RaycastResult> results = new List<RaycastResult>(); // List�� ����
        EventSystem.current.RaycastAll(pointerEventData, results);

        if (results.Count > 0)
        {
            // UI ��Ҹ� Ŭ������ ���� ó���� ���⿡ �߰�
            GameObject clickedUIElement = results[0].gameObject;

            // ���⿡ ������ clikedUIElement �������� ���� ������ �����س��� �ɰŰ���
            Debug.Log("Ŭ���� UI ���: " + clickedUIElement.tag);


            if(clickedUIElement.CompareTag("ItemSlot")) // ������ ������ ���������
            {
                giveItemSlotClass = clickedUIElement.GetComponent<SG_ItemSlot>();
                //���� �����۽����� ������ȣ ����
                giveSlotCount = giveItemSlotClass.slotCount;
                Debug.LogFormat("Ŭ���� ������ ������ȣ -> {0}", giveSlotCount);
            }
            
        }
        else
        {
            // UI ��Ұ� ���ų� Ŭ������ �ʾ��� ���� ó���� ���⿡ �߰�
            Debug.Log("ClickIn() UI ��Ҹ� Ŭ������ �ʾҰų�, Ŭ���� UI ��Ұ� �����ϴ�.");
        }
    }





}   // NAMESPACE

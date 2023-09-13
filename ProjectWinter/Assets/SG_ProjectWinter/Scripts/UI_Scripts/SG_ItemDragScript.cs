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

    // 23.09.13 Drag �ʿ��� ����� �ҰŰ���

    private SG_ItemSlot playerItemSlotClass;
    private SG_WareHouseItemSlot werehouseItemSlotClass;
    private SG_ItemSwapManager swapManagerClass;

    // �Ʒ����� �ڽŰ� ��븦 �����ϱ� ���� Class ����
    private SG_ItemSlot giveByPlayer;
    private SG_WareHouseItemSlot giveByWareHouse;
    //perent = this.transform.parent.gameObject.GetComponent<SG_ItemSlot>();

    // �ӽ� �ִ� ���� �޴� ���� ������ȣ ���⼭ �޾Ƽ� �Ѱ��� ����
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
    /// ���� ������Ʈ�� �巡���ϱ� ������ �� 1ȸ ȣ��
    /// </summary>    
    public void OnBeginDrag(PointerEventData eventData)
    {
        // �巡�� ������ �ҼӵǾ� �ִ� �θ� Transform ���� ����
        previousParent = this.transform.parent;

        // ���� �巡������ UI�� ȭ���� �ֻ�ܿ� ��µǵ��� �ϱ� ����
        this.transform.SetParent(canvasTrans);  // �θ� ������Ʈ�� Canvas�� ����
        transform.SetAsLastSibling();           // ���� �տ� ���̵��� ������ �ڽ����� ����

        // �巡�� ������ ������Ʈ�� �ϳ��� �ƴ� �ڽĵ��� ������ ���� ���� �ֱ� ������ CanvasGroup����
        // ���İ��� 0.6���� �����ϰ�, ���� �浹ó���� ���� �ʵ��� �Ѵ�.
        //canvasGroup.alpha = 0.6f;
        //canvasGroup.blocksRaycasts = false;

        // Ŭ���� ������ ������ȣ ������ ���� �Լ�
        ClickDown();
        itemImage.raycastTarget = false;

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


        //TODO : �Ʒ��� �Ű������� �÷��̾ â��� �ű�°��ε� ���� ���εδ��� �������� �־ ���� �־����� �νĽ��Ѿ�
        // �ٹ������ ��밡���ҰŰ���
        // �� ���� ��� 1 -> �ڽ��� �θ������Ʈ�� tag�� ���� �ִ½�ũ��Ʈ�� �޴� ��ũ��Ʈ�� �����Ѵ�
        //  Ŭ������ ������ �� ������������ ����


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
            if(clickedUIElement.gameObject.CompareTag("WareHouseSlot"))
            {
                werehouseItemSlotClass = clickedUIElement.GetComponent<SG_WareHouseItemSlot>();
                acceptSlotCount = werehouseItemSlotClass.wareHouseSlotCount;
            }

            // Ŭ���� UI ��ҿ� ���� �۾��� ������ �� �ֽ��ϴ�.
        }
        else
        {
            // UI ��Ұ� ���ų� Ŭ������ �ʾ��� ���� ó���� ���⿡ �߰�
            Debug.Log("Click Up UI ��Ҹ� Ŭ������ �ʾҰų�, Ŭ���� UI ��Ұ� �����ϴ�.");
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

        

        // �̰����� �ڽ��� �������� �������� ����� �������� �˼� ����
        // ���⼭ Swap�� ���ָ� �ɰŰ��⵵��
        if(giveByPlayer != null) //������ ���µ� ������ �÷��̾��� ���̸�
        {
            // �Ű����� ���� : �ִ¾� �ּ�, �޴¾��ּ� , �÷��̾� ����, �κ��丮 ����
            swapManagerClass.ItemSwap(giveSlotCount, acceptSlotCount, playerItemSlotClass, werehouseItemSlotClass);
            giveByPlayer = null;
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
            Debug.Log("Ŭ���� UI ���: " + clickedUIElement.name);


            if(clickedUIElement.CompareTag("PlayerSlot")) // �÷��̾� ������ ���������
            {
                playerItemSlotClass = clickedUIElement.GetComponent<SG_ItemSlot>();
                //�����÷��̾��� ������ȣ ����
                giveSlotCount = playerItemSlotClass.slotCount;
            }

            

            // Ŭ���� UI ��ҿ� ���� �۾��� ������ �� �ֽ��ϴ�.
        }
        else
        {
            // UI ��Ұ� ���ų� Ŭ������ �ʾ��� ���� ó���� ���⿡ �߰�
            Debug.Log("ClickIn() UI ��Ҹ� Ŭ������ �ʾҰų�, Ŭ���� UI ��Ұ� �����ϴ�.");
        }
    }





}   // NAMESPACE

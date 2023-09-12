using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SG_InventoryClickScript : MonoBehaviour,IPointerEnterHandler,IDropHandler,IPointerExitHandler
   
{
    
    private Image image;
    private RectTransform rect;   
    private Color32 defaultColor32;

    private SG_ItemSlot playerItemSlotClass;
    private SG_WareHouseItemSlot werehouseItemSlotClass;
    private SG_ItemSwapManager swapManagerClass;

    private void Awake()
    {
       image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();       
        defaultColor32 = new Color32(180, 180, 180, 255);
        swapManagerClass = GetComponent<SG_ItemSwapManager>();


    }

    /// <summary>
    /// ���콺 �����Ͱ� ���� ������ ���� ���� ���η� �� �� 1ȸ ȣ��
    /// </summary>    
    public void OnPointerEnter(PointerEventData eventData)
    {
        // ������ ������ ������ ��������� ����
        image.color = Color.yellow;
    }

  
    /// <summary>
    /// ���� ������ ���� ���� ���ο��� ����� ���� �� 1ȸ ȣ��
    /// </summary>  
    public void OnDrop(PointerEventData eventData)
    {
        // PointerDrag�� ���� �巡�� �ϰ� �ִ� ���(= ������)
        if(eventData.pointerDrag != null)
        {
            // �巡�� �ϰ� �ִ� ����� �θ� ���� ������Ʈ�� �����ϰ�, ��ġ�� ���� ������Ʈ ��ġ�� �����ϰ� ����
            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;

            // ���⿡ SwapClass ���� ������ ���� ��� �ִ� ���� ���־�� �ҰŰ���
            playerItemSlotClass = this.GetComponent<SG_ItemSlot>();
            werehouseItemSlotClass = eventData.pointerDrag.transform.GetComponent<SG_WareHouseItemSlot>();
            //swapManagerClass.ItemSwap(playerItemSlotClass.slotCount, werehouseItemSlotClass.slotCount);
        }
    }

    /// <summary>
    /// ���콺 �����Ͱ� ���� ������ ���� ������ �������� �� 1ȸ ȣ��
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        // TODO : �Ʒ� defaultColor �� �ٲٴµ� ���� â��� ���������� �ٲ����� ���� �߰��������

        // ������ ������ ������ ���������� ����
        image.color = defaultColor32;
        //image.color = Color.black;
    }





    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    // Ŭ���� �������� �Ҹ� �Լ� Canvas�� UI���Ը� �Ҹ��� �������̽� ���
    //    Debug.Log("Ŭ���������� �Ҹ���?");
    //   Debug.LogFormat("pointerDrag�� �����ΰ�? -> {0}", eventData.pointerDrag);
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    // �巡���߿� �Ҹ� �Լ�
    //    Debug.Log("�巡���߿� ��� �Ҹ���?");

    //    // ������ �̹����� �巡���߿� ��� ���콺�� ���󰡵��� ������ ����
    //    transform.position = eventData.position;
    //}

    //public void OnDrop(PointerEventData eventData)
    //{
    //    // ���콺Ŭ���� UI ������ �������� ������ �Լ�
    //    Debug.LogFormat("pointerDrag�� �����ΰ�? -> {0}", eventData.pointerDrag);
    //    //if(Physics.Raycast())
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    // �巡�׸� �� ���콺Ŭ���� �������� ������ �θ��� �Լ�
    //    //Debug.Log("�巡�� ��ġ�� ���콺Ŭ���� ���� �Ҹ���?");
    //}
}

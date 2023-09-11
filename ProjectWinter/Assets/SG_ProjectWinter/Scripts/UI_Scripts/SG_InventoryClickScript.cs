using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SG_InventoryClickScript : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
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

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / transform.lossyScale.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
     
    }

    public void OnDrop(PointerEventData eventData)
    {
        Image image = eventData.pointerDrag.gameObject.GetComponent<Image>();
        Debug.LogFormat("pointerDrag -> {0}", image.sprite.name);
        if (eventData.pointerDrag == null)
            
            return;

        // ���� ������ ����
        //InventoryItem draggedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        //if (draggedItem == null)
        //    return;

        // �巡���� ������ ������ ��ġ
        RectTransform otherSlotRectTransform = GetComponent<RectTransform>();

        // Swap ����
        Vector3 tempPosition = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = otherSlotRectTransform.anchoredPosition;
        otherSlotRectTransform.anchoredPosition = tempPosition;
    }
}

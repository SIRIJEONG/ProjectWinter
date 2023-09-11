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

    private Vector3 tempPos;

    private RectTransform rectTransform;
    private Transform originalParent; // �������� ���� �θ�
    private Vector3 originalPosition; // �������� ���� ��ġ

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = transform.position;
        
        Debug.Log(eventData.pointerDrag.gameObject.name);
        //Debug.Log(tempPos);
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
        if (eventData.pointerDrag != null)
        {
            // ����� ��ġ�� UI ��Ҹ� �˻�
            RectTransform dropTarget = eventData.pointerEnter.GetComponent<RectTransform>();

            if (dropTarget != null)
            {
                // �θ� ���� �θ�� ����
                transform.SetParent(originalParent);
                // �������� ���� ��ġ�� ������
                transform.position = originalPosition;
            }
        }

            //eventData.pointerDrag.gameObject.transform.position = tempPos;

            //if (eventData.pointerDrag == null)
            //{

            //    return;
            //}
            //else if(eventData.pointerDrag != null)
            //{
            //    Image image = eventData.pointerDrag.gameObject.GetComponent<Image>();
            //    //Debug.LogFormat("pointerDrag -> {0}", image.sprite.name);
            //}


            // ���� ������ ����
            //InventoryItem draggedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            //if (draggedItem == null)
            //    return;

            //// �巡���� ������ ������ ��ġ
            //RectTransform otherSlotRectTransform = GetComponent<RectTransform>();

            //// Swap ����
            //Vector3 tempPosition = rectTransform.anchoredPosition;
            //rectTransform.anchoredPosition = otherSlotRectTransform.anchoredPosition;
            //otherSlotRectTransform.anchoredPosition = tempPosition;
        }
}

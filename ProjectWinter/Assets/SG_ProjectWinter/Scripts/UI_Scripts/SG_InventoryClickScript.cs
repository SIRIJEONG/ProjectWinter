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
    //    // 클릭을 했을때에 불릴 함수 Canvas속 UI에게만 불리는 인터페이스 사용
    //    Debug.Log("클릭했을떄에 불리나?");
    //   Debug.LogFormat("pointerDrag는 무엇인가? -> {0}", eventData.pointerDrag);
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    // 드래그중에 불릴 함수
    //    Debug.Log("드래그중에 계속 불리나?");

    //    // 아이템 이미지가 드래그중에 계속 마우스를 따라가도록 포지션 조정
    //    transform.position = eventData.position;
    //}

    //public void OnDrop(PointerEventData eventData)
    //{
    //    // 마우스클릭을 UI 위에다 땟을때에 나오는 함수
    //    Debug.LogFormat("pointerDrag는 무엇인가? -> {0}", eventData.pointerDrag);
    //    //if(Physics.Raycast())
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    // 드래그를 후 마우스클릭을 끝낼때에 무조건 부르는 함수
    //    //Debug.Log("드래그 마치고 마우스클릭을 때면 불리나?");
    //}

    private Vector3 tempPos;

    private RectTransform rectTransform;
    private Transform originalParent; // 아이템의 원래 부모
    private Vector3 originalPosition; // 아이템의 원래 위치

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
            // 드롭한 위치의 UI 요소를 검사
            RectTransform dropTarget = eventData.pointerEnter.GetComponent<RectTransform>();

            if (dropTarget != null)
            {
                // 부모를 원래 부모로 변경
                transform.SetParent(originalParent);
                // 아이템을 원래 위치로 돌리기
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


            // 현재 아이템 슬롯
            //InventoryItem draggedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            //if (draggedItem == null)
            //    return;

            //// 드래그한 아이템 슬롯의 위치
            //RectTransform otherSlotRectTransform = GetComponent<RectTransform>();

            //// Swap 로직
            //Vector3 tempPosition = rectTransform.anchoredPosition;
            //rectTransform.anchoredPosition = otherSlotRectTransform.anchoredPosition;
            //otherSlotRectTransform.anchoredPosition = tempPosition;
        }
}

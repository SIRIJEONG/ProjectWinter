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
    /// 마우스 포인터가 현재 아이템 슬롯 영역 내부로 들어갈 때 1회 호출
    /// </summary>    
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 아이템 슬롯의 색상을 노란색으로 변경
        image.color = Color.yellow;
    }

  
    /// <summary>
    /// 현재 아이템 슬롯 영역 내부에서 드롭을 했을 때 1회 호출
    /// </summary>  
    public void OnDrop(PointerEventData eventData)
    {
        // PointerDrag는 현재 드래그 하고 있는 대상(= 아이템)
        if(eventData.pointerDrag != null)
        {
            // 드래그 하고 있는 대상의 부모를 현재 오브젝트로 설정하고, 위치를 현재 오브젝트 위치와 동일하게 설정
            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;

            // 여기에 SwapClass 에게 아이템 정보 들고 있는 것을 쏴주어야 할거같음
            playerItemSlotClass = this.GetComponent<SG_ItemSlot>();
            werehouseItemSlotClass = eventData.pointerDrag.transform.GetComponent<SG_WareHouseItemSlot>();
            //swapManagerClass.ItemSwap(playerItemSlotClass.slotCount, werehouseItemSlotClass.slotCount);
        }
    }

    /// <summary>
    /// 마우스 포인터가 현재 아이템 슬롯 영역을 빠져나갈 때 1회 호출
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        // TODO : 아래 defaultColor 로 바꾸는데 산장 창고는 검은색으로 바뀌어야함 조건 추가해줘야함

        // 아이템 슬롯의 색상을 원래색으로 변경
        image.color = defaultColor32;
        //image.color = Color.black;
    }





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
}

using System.Collections;
using System.Collections.Generic;
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

        // 알파값을 1로 설정하고, 광선 충돌처리가 되도록 한다.
        //canvasGroup.alpha = 1.0f;
        //canvasGroup.blocksRaycasts = true;

        itemImage.raycastTarget = true;
    }

    public void OnDrop(PointerEventData eventData)
    {

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

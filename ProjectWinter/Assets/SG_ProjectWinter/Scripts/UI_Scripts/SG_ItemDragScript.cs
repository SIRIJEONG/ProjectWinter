using System.Collections;
using System.Collections.Generic;
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

        // ���İ��� 1�� �����ϰ�, ���� �浹ó���� �ǵ��� �Ѵ�.
        //canvasGroup.alpha = 1.0f;
        //canvasGroup.blocksRaycasts = true;

        itemImage.raycastTarget = true;
    }

    public void OnDrop(PointerEventData eventData)
    {

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

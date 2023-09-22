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

    private SG_ItemSlot itemSlotClass;

    private bool isInIt = false;

    private void Awake()
    {
        FirstInIt();
    }

    private void FirstInIt()
    {
        if (isInIt == false)
        {
            isInIt = true;
            image = GetComponent<Image>();
            rect = GetComponent<RectTransform>();
            defaultColor32 = new Color32(180, 180, 180, 255);
            itemSlotClass = this.transform.GetComponent<SG_ItemSlot>();
        }
        else { /*PASS*/ }
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
            if(itemSlotClass.item == null)
            {
                // �巡�� �ϰ� �ִ� ����� �θ� ���� ������Ʈ�� �����ϰ�, ��ġ�� ���� ������Ʈ ��ġ�� �����ϰ� ����            
                eventData.pointerDrag.transform.SetParent(transform);
                eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;
            }

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
        //Debug.Log(this.gameObject.tag);
        if(itemSlotClass.slotTopParentObj.gameObject.CompareTag("Warehouse"))
        {
            image.color = Color.black;
        }
        else if(itemSlotClass.slotTopParentObj.gameObject.CompareTag("PowerStation"))
        {
            image.color = Color.black;
        }
        else if(itemSlotClass.slotTopParentObj.gameObject.CompareTag("HeliPad"))
        {
            image.color = Color.black;
        }
        //image.color = Color.black;
    }


}

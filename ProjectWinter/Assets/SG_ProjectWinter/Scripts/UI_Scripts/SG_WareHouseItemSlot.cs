using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SG_WareHouseItemSlot : MonoBehaviour
{
    // LEGACY  : ItemSlot �ѽ�ũ��Ʈ�� ����    23.09.26

    // 23.09.10 �Ʒ� �� �÷��̾��� ItemSlot�� �״�� �����°���
    // �Ʒ������� ���� ����â�� �������� �� �ٲ㼭 ����ؾ� ��
    // 23.09.10 �ѹ� �߰� ���忡 ����ϴµ��� ū ������ �־���� �ʱ⿡ �ϴ� ���

    public SG_Item item;    // �������� ������ ����ִ� ��
    public int itemCount;   // ȹ���� �������� ����
    public Image itemImage; // ȹ���� �������� �̹���
    public int wareHouseSlotCount;

    // �ʿ��� ������Ʈ
    [SerializeField]
    private TextMeshProUGUI text_Count;
    [SerializeField]
    private GameObject itemCountImg;


    // �̹����� ���� ����
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }


    // ������ ȹ��
    public void AddItem(SG_Item _item, int _count = 1)
    {
        //TODO : ������ ��ũ��Ʈ ������ ������ ���������
        //          �߰����ִ��Լ� �� �־��־����
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if (item.itemType != SG_Item.ItemType.Weapon)
        {
            itemCountImg.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else
        {
            text_Count.text = "0";
            itemCountImg.SetActive(false);
        }

        SetColor(1);
    }

    // ������ ���� ����
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }

    // ������ ���� �ʱ�ȭ
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        itemCountImg.SetActive(false);
    }

}

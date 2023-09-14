using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SG_ItemSlot : MonoBehaviour
{

    public SG_Item item;    // �������� ������ ����ִ� ��
    public int itemCount;   // ȹ���� �������� ����
    public int slotCount = 0;   //������ ������ ������ȣ ���Ե� ����

    public GameObject itemImagePrefab;

    private GameObject itemImageClone;

    // �Ʒ� �̹����� �������� �Ծ������� Instance �ǰ� Instance �������� �ڽĿ�����Ʈ�� ã�Ƽ� �־��ٰ���
    public Image itemImage; // ȹ���� �������� �̹���


    // �ʿ��� ������Ʈ 
    [SerializeField]
    private TextMeshProUGUI text_Count;
    private GameObject itemCountImg;

    private void Start()
    {
        
    }


    // �̹����� ���� ����
    private void SetColor(float _alpha)
    {
        //Color color = itemImage.color;
        //color.a = _alpha;
        //itemImage.color = color;
    }


    // ������ ȹ��
    public void AddItem(SG_Item _item, int _count = 1)
    {
        //TODO : ������ ��ũ��Ʈ ������ ������ ���������
        //          �߰����ִ��Լ� �� �־��־����
        item = _item;
        itemCount = _count;
        //Debug.Log("������ =");
        //itemImage.sprite = item.itemImage;

        if (item.itemType != SG_Item.ItemType.Weapon)
        {
            if (itemImage == null)
            {
                ImageObjInstance();
            }

            //itemCountImg.SetActive(true);
            text_Count.text = itemCount.ToString();
            itemImage.sprite = item.itemImage;
        }
        else
        {
            text_Count.text = "0";
            //itemCountImg.SetActive(false);
        }

        //Debug.Log(slotCount);

        SetColor(1);
    }

    // ������ ���� ����
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();
        //Debug.Log("������ +=");

        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }

    // ������ ���� �ʱ�ȭ
    public void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
       // itemCountImg.SetActive(false);
    }

    // ItemImageObj�� �ν��Ͻ��� ������ �ڽĿ�����Ʈ�� �־��ִ� �Լ�
    private void ImageObjInstance()
    {
        itemImageClone = Instantiate(itemImagePrefab, this.transform.position, Quaternion.identity);
        itemImageClone.transform.SetParent(this.transform);

        ChildSet();
    }

    // ������ Image Ŭ�� ������ �ڽ��� �ڽ��� �ڽ� ���� ã�Ƽ� ����־��ֱ�
    private void ChildSet()
    {
        GameObject tempObj;
        tempObj = this.gameObject.transform.GetChild(0).gameObject;
        itemImage = tempObj.GetComponent<Image>();

        GameObject tempObj002;
        tempObj002 = tempObj.gameObject.transform.GetChild(0).gameObject;
        itemCountImg = tempObj002.GetComponent<GameObject>();

        // ������ tempObj ��Ȱ��
        tempObj = tempObj002.gameObject.transform.GetChild(0).gameObject;
        text_Count = tempObj.GetComponent<TextMeshProUGUI>();
    }

}

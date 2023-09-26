using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SG_ItemSlot : MonoBehaviour
{
    private SG_ItemDragScript dragScript;
    public SG_Item item;    // �������� ������ ����ִ� ��
    public int itemCount = 0;   // ȹ���� �������� ����
    public int slotCount = 0;   //������ ������ ������ȣ ���Ե� ����

    public GameObject itemImagePrefab;

    private GameObject itemImageClone;

    // �Ʒ� �̹����� �������� �Ծ������� Instance �ǰ� Instance �������� �ڽĿ�����Ʈ�� ã�Ƽ� �־��ٰ���
    public Image itemImage; // ȹ���� �������� �̹���


    // �ʿ��� ������Ʈ     
    public TextMeshProUGUI text_Count;
    public GameObject itemCountImg;
    public Image itemCountImage;

    public GameObject slotTopParentObj;

    private Color defaultColor;     // Color 1,1,1,1 ��
    private Color transparentColor; // Color 1,1,1,0 ��
    private Color weaponColorSet;   // �����϶��� A�� �����ϰ� ���� �÷� ����

    private Coroutine TextSpriteUpdateCoroutine;    // SetItemSprite_Count() �� ����� StartCoroutine�ڽ�

    private void Start()
    {
        StartInIt();
    }

   private void StartInIt()
    {
        slotTopParentObj = this.transform.parent.gameObject;
        // slot�� �ֻ��� ������Ʈ �±׷� ������ �����ϱ����� �ֻ��� ������Ʈ �������ִ� �Լ�
        GetThisTopParentObj();

        weaponColorSet = new Color(1f, 1f, 1f, 0f);
        transparentColor = new Color(1f, 1f, 1f, 0f);
        defaultColor = new Color(1f, 1f, 1f, 1f);
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
        // �÷��̾� �� �ƴ϶�� Inventory Script ���� Return�� ������ ������ ���ǹ� ���� X
    

        item = _item;
        itemCount = _count;
        //itemImage.sprite = item.itemImage;

        if (item.itemType != SG_Item.ItemType.Weapon)
        {
            if (itemImage == null)
            {
                ImageObjInstance();
            }

            text_Count.text = itemCount.ToString();
            itemImage.sprite = item.itemImage;
        }
        else
        {
            if (itemImage == null)
            {
                ImageObjInstance();
            }

            text_Count.text = itemCount.ToString();
            itemImage.sprite = item.itemImage;
            WeaponColorSet();

        }

        SetColor(1);
    }

    // ���� �������� ���� �϶��� ������ Text�� CountImage �� ����
    public void WeaponColorSet()
    {
        text_Count.color = weaponColorSet;
        itemCountImage.color = weaponColorSet;

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

    // ItemImageObj�� �ν��Ͻ��� ������ �ڽĿ�����Ʈ�� �־��ִ� �Լ�
    public void ImageObjInstance()
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
        itemCountImage = tempObj002.GetComponent<Image>();
        itemCountImg = tempObj002.gameObject;

        // ������ tempObj ��Ȱ��
        tempObj = tempObj002.gameObject.transform.GetChild(0).gameObject;
        text_Count = tempObj.GetComponent<TextMeshProUGUI>();
    }

    public void MoveItemSet()
    {

        if (item != null && this.gameObject.transform.childCount > 0)
        {
            //Debug.LogFormat("������ ���۽� ������� ������?");
            GameObject tempObj;
            tempObj = this.gameObject.transform.GetChild(0).gameObject;
            itemImage = tempObj.GetComponent<Image>();

            GameObject tempObj002;
            tempObj002 = tempObj.gameObject.transform.GetChild(0).gameObject;
            itemCountImage = tempObj002.GetComponent<Image>();
            itemCountImg = tempObj002.gameObject;

            // ������ tempObj ��Ȱ��
            tempObj = tempObj002.gameObject.transform.GetChild(0).gameObject;
            text_Count = tempObj.GetComponent<TextMeshProUGUI>();

            TextSpriteUpdateCoroutine = StartCoroutine(SetItemSprite_Count());

        }
        else { /*PASS*/ }
    }

    // ã�Ƴ��� �����۰��� ���� �����ִ� �Լ� -> �̹���,ī��Ʈ�ؽ�Ʈ,ī��Ʈ�̹���
    public void DisconnectedItem()
    {
        //text_Count.text = "0";
        text_Count = null;
        //itemImage.sprite = null;        
        itemImage = null;
        itemCountImg = null;
        item = null;
        itemCount = 0;
        Transform childTrans;
        if (this.transform.childCount > 0)
        {
            childTrans = this.transform.GetChild(0);
            Destroy(childTrans.gameObject);
            SG_ItemDragScript.allItemDragScrip.Remove(childTrans.GetComponent<SG_ItemDragScript>());
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


    //�ֻ��� �θ� ������Ʈ �±׸� �������� ���� ã�� ����
    private void GetThisTopParentObj()
    {        
        //�ֻ��� �θ� ������Ʈ �±׸� �������� ���� ã�� ����
        while (slotTopParentObj.transform.parent != null)
        {          
            slotTopParentObj = slotTopParentObj.transform.parent.gameObject;
        }
    }

    private void ItemImageGetScript()
    {
        dragScript = itemImage.GetComponent<SG_ItemDragScript>();
        UpdateCanvas(); // ������ �̹����� ĵ���� ������Ʈ ���ִ� �Լ�
    }

    private void UpdateCanvas()
    {
        dragScript.CanvasUpdate();
    }

    private IEnumerator SetItemSprite_Count()
    {
        itemImage.color = transparentColor; // ������ �����ϰ� �ؼ� �� �ڽ� �Ⱥ��̰�
        yield return null;
        itemImage.color = defaultColor;     // ������ �� �������
        // ��������Ʈ�� �ؽ�Ʈ ���

        //Debug.LogFormat("�ڷ�ƾ ���������� ItemCount -> {0}", itemCount);
        text_Count.text = itemCount.ToString();
        itemImage.sprite = item.itemImage;

        // Canvas�缳��
        ItemImageGetScript();  //��ũ��Ʈ �������� �õ�

    }

    public void TextUpdate()    // ������ ī��Ʈ �ؽ�Ʈ�� ������Ʈ ���ִ� �Լ�
    {
        text_Count.text = itemCount.ToString();
    }

}   // NameSpace

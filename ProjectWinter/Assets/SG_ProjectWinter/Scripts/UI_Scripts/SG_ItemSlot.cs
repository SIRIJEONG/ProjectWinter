using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SG_ItemSlot : MonoBehaviour
{

    public SG_Item item;    // 아이템의 정보가 들어있는 곳
    public int itemCount;   // 획득한 아이템의 갯수
    public int slotCount = 0;   //아이템 슬롯의 고유번호 삽입될 변수

    public GameObject itemImagePrefab;

    private GameObject itemImageClone;

    // 아래 이미지는 아이템을 먹었을때에 Instance 되고 Instance 됬을떄에 자식오브젝트로 찾아서 넣어줄거임
    public Image itemImage; // 획득한 아이템의 이미지


    // 필요한 컴포넌트 
    [SerializeField]
    private TextMeshProUGUI text_Count;
    private GameObject itemCountImg;

    private void Start()
    {
        
    }


    // 이미지의 투명도 조절
    private void SetColor(float _alpha)
    {
        //Color color = itemImage.color;
        //color.a = _alpha;
        //itemImage.color = color;
    }


    // 아이템 획득
    public void AddItem(SG_Item _item, int _count = 1)
    {
        //TODO : 아이템 스크립트 만든후 아이템 얻었을떄에
        //          추가해주는함수 에 넣어주어야함
        item = _item;
        itemCount = _count;
        //Debug.Log("아이템 =");
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

    // 아이템 개수 조정
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();
        //Debug.Log("아이템 +=");

        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }

    // 아이템 슬롯 초기화
    public void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
       // itemCountImg.SetActive(false);
    }

    // ItemImageObj를 인스턴스후 슬롯의 자식오브젝트로 넣어주는 함수
    private void ImageObjInstance()
    {
        itemImageClone = Instantiate(itemImagePrefab, this.transform.position, Quaternion.identity);
        itemImageClone.transform.SetParent(this.transform);

        ChildSet();
    }

    // 아이템 Image 클론 만든후 자식의 자식의 자식 까지 찾아서 집어넣어주기
    private void ChildSet()
    {
        GameObject tempObj;
        tempObj = this.gameObject.transform.GetChild(0).gameObject;
        itemImage = tempObj.GetComponent<Image>();

        GameObject tempObj002;
        tempObj002 = tempObj.gameObject.transform.GetChild(0).gameObject;
        itemCountImg = tempObj002.GetComponent<GameObject>();

        // 맨위에 tempObj 재활용
        tempObj = tempObj002.gameObject.transform.GetChild(0).gameObject;
        text_Count = tempObj.GetComponent<TextMeshProUGUI>();
    }

}

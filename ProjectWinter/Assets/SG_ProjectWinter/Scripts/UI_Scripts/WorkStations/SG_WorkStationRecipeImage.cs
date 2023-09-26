using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SG_WorkStationRecipeImage : MonoBehaviour, IPointerClickHandler
{
    public SG_ItemRecipe itemRecipe;
    private SG_WorkStationContentControler workStationContentControlerClass;

    private Image madeItemImage;        //완성아이템의 이미지
    private Image materialItemImage001; //재료아이템의 이미지    
    private Image materialItemImage002; //재료아이템의 이미지
    private Image thisImage;    //레시피 뒷 배경이미지

    private Color defaultColor; // Defaul Color 204 204 204 204
    private Color onClickColor; // OnClick Color 102 102 102 204

    public int recipeCount;

    public event System.Action<int> RecipeListClickEvent;

    private bool isMaterialItem002Null; // 아이템 재료가 2개가 아니라 한개일경우 2번째 이미지 꺼줄 bool 변수


    public bool isClickState = false;

    private void Awake()
    {
        AwakeInIt();
    }

    void Start()
    {
        StartInIt();
    }

    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)  // 클릭시 1회 호출
    {
        RecipeListClickEvent?.Invoke(recipeCount);
    }


    private void AwakeInIt()    // Awake단계에서 넣어줄 변수
    {

        workStationContentControlerClass = transform.parent.GetComponent<SG_WorkStationContentControler>();
        madeItemImage = transform.Find("ItemImage").GetComponent<Image>();
        materialItemImage001 = transform.Find("NeedItemImage001").GetComponent<Image>();
        materialItemImage002 = transform.Find("NeedItemImage002").GetComponent<Image>();
        thisImage = GetComponent<Image>();
    }

    private void StartInIt()    // Start단계에서 넣어줄 변수
    {
        CheckMeterialItem002Null(); // 아이템 재료2가 존재하는지 체크하는 함수
        madeItemImage.sprite = itemRecipe.madeItem.itemImage;
        materialItemImage001.sprite = itemRecipe.item001.itemImage;

        CheckOutPutItemImage(); // 아이템 재료2 가 존재하면 출력하고 존재하지 않는다면 칸을 SetActive(false) 해주는 함수

        defaultColor = new Color(0.8f, 0.8f, 0.8f, 0.8f);
        onClickColor = new Color(0.4f, 0.4f, 0.4f, 0.8f);

        workStationContentControlerClass.RecipeListSetColorEvent += SetImageColor;
    }

    public void SetImageColor() //클릭되있는지 확인하고 상태에 따라서 색이바뀌는 함수
    {
        if (isClickState == true)
        {
            thisImage.color = onClickColor;
        }
        else if (isClickState == false)
        {
            thisImage.color = defaultColor;
        }

    }

    private void CheckMeterialItem002Null()
    {
        if (itemRecipe.item002 == null)
        {
            isMaterialItem002Null = true;
        }

        else if (itemRecipe.item002 != null)
        {
            isMaterialItem002Null = false;
        }

        else { /*PASS*/ }
    }

    // 아이템 재료2 가 존재하면 출력하고 존재하지 않는다면 칸을 SetActive(false) 해주는 함수
    private void CheckOutPutItemImage()
    {
        if (isMaterialItem002Null == false)
        {
            materialItemImage002.sprite = itemRecipe.item002.itemImage;
        }
        else if (isMaterialItem002Null == true)
        {
            materialItemImage002.gameObject.SetActive(false);
        }
    }


}   //NameSpacve

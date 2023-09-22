using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SG_WorkStationRecipeImage : MonoBehaviour, IPointerClickHandler
{
    public SG_ItemRecipe itemRecipe;
    private SG_WorkStationContentControler workStationContentControlerClass;

    private Image madeItemImage;        //�ϼ��������� �̹���
    private Image materialItemImage001; //���������� �̹���    
    private Image materialItemImage002; //���������� �̹���
    private Image thisImage;    //������ �� ����̹���

    private Color defaultColor; // Defaul Color 204 204 204 204
    private Color onClickColor; // OnClick Color 102 102 102 204

    public int recipeCount;

    public event System.Action<int> RecipeListClickEvent;




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

    public void OnPointerClick(PointerEventData eventData)  // Ŭ���� 1ȸ ȣ��
    {       
        RecipeListClickEvent?.Invoke(recipeCount);      
    }


    private void AwakeInIt()    // Awake�ܰ迡�� �־��� ����
    {
        workStationContentControlerClass = transform.parent.GetComponent<SG_WorkStationContentControler>();
        madeItemImage = transform.Find("ItemImage").GetComponent<Image>();
        materialItemImage001 = transform.Find("NeedItemImage001").GetComponent<Image>();
        materialItemImage002 = transform.Find("NeedItemImage002").GetComponent<Image>();
        thisImage = GetComponent<Image>();        
    }

    private void StartInIt()    // Start�ܰ迡�� �־��� ����
    {
        madeItemImage.sprite = itemRecipe.madeItem.itemImage;
        materialItemImage001.sprite = itemRecipe.item001.itemImage;
        materialItemImage002.sprite = itemRecipe.item002.itemImage;

        defaultColor = new Color(0.8f, 0.8f, 0.8f, 0.8f);
        onClickColor = new Color(0.4f, 0.4f, 0.4f, 0.8f);

        workStationContentControlerClass.RecipeListSetColorEvent += SetImageColor;
    }

    public void SetImageColor() //Ŭ�����ִ��� Ȯ���ϰ� ���¿� ���� ���̹ٲ�� �Լ�
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


}   //NameSpacve

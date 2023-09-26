using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SG_WorkStationContentControler : MonoBehaviour
{
    private GameObject itemRecipeListParent;    // �����۷������� �θ� ������Ʈ -> this

    public SG_WorkStationRecipeImage[] itemRecipeList;  // ���۰����� �����ǵ��� ��Ƶ� �迭

    public event System.Action RecipeListSetColorEvent; // Ŭ���� �����ǻ��� �������� �̺�Ʈ

    private Transform topParentTrans;               // �ֻ��� �θ� �־��� ����
    private SG_WorkStationControler topParentClass; // topParent�� ������ �ִ� Class Inventory�� ���� ��Ȱ�� ���ִ°��� ����������
    public SG_Inventory playerInventory;            // ������ �������Ҷ� ��Ḧ �Һ��� �÷��̾��� �κ��丮

    private SG_ProductionManager productionManager; // ���� �޴����� �Լ��� ȣ���ϱ� ���� ����

    private int itemRecipeListCount;    // �����۷����� ����Ʈ �����ً� ��Ȯ�� �����ֱ����� ����


    void Awake()
    {
        AwakeInIt();        // Awake �ܰ迡�� ������ ����
        SetRecipeCount();   // ������ ������ ����Ʈ�� ������ȣ �־���
        EventSubscriber();   // ������ �������� �̺�Ʈ�� �������ִ� �Լ�
    }

    void Start()
    {
        StartInIt();        //Start �ܰ迡�� ������ ����
        
    }
    
    void Update()
    {
        
    }

    public void AwakeInIt()
    {
        itemRecipeListParent = GetComponent<Transform>().gameObject;        
        itemRecipeList = itemRecipeListParent.GetComponentsInChildren<SG_WorkStationRecipeImage>();
    }

    public void StartInIt() // Start�������� �־��� ��
    {
        productionManager = FindAnyObjectByType<SG_ProductionManager>();
    }

    private void SetRecipeCount()   // �������� ������ȣ�־��ִ� �Լ�
    {
        // ��ȣ�� I���� �־ �迭�״�� ���� �����Ű���
        for(int i =0; i < itemRecipeList.Length; i++)
        {
            itemRecipeList[i].recipeCount = i;
        }
    }

    public void EventSubscriber() // �̺�Ʈ�� �������ִ� �Լ�
    {
        for(int i =0; i < itemRecipeList.Length; i++)
        {
            itemRecipeList[i].RecipeListClickEvent += RecipeListClickEventCall;
        }
    }

    public void RecipeListClickEventCall(int _ListCount)
    {
        for(int i = 0; i < itemRecipeList.Length; i++)
        {
            if (itemRecipeList[i].recipeCount == _ListCount)
            {
                itemRecipeList[i].isClickState = true;
                //Debug.LogFormat("���� �����ִ� ������ -> {0}", itemRecipeList[i].itemRecipe.madeItem.itemName);
            }
            else
            {
                itemRecipeList[i].isClickState = false;
            }
        }

        RecipeListSetColorEvent?.Invoke();  // �÷� bool ���� ����ٲ�� �Լ� ����

    }



    private void SerchTopParentTrans()  // �ֻ��� �θ������Ʈ ã�� �Լ�
    {
        topParentTrans = transform;

        while(topParentTrans.parent != null)
        {
            topParentTrans = topParentTrans.parent.transform;
        }
    }

    public void LetProduction() //���۹�ư�� ������ ����� �Լ�
    {
        SerchTopParentTrans();
        SerchMakeItemList();    // ��ư ���� ������ Ŭ�����°� true�Ǿ��ִ� ������ ����

        topParentClass = topParentTrans.GetComponent<SG_WorkStationControler>();

        playerInventory = topParentClass.playerInventory;   // �ֻ����θ� ��Ƶΰ� �ִ� �÷��̾��� �κ��丮 ��ư���� �佺

        productionManager.StartProduction(playerInventory, itemRecipeList[itemRecipeListCount]);    // ������ ���� �������� ���

    }

    private void SerchMakeItemList()    // ���۹�ư �������� �����ǵ��� ��ũ��Ʈ���鼭 Ŭ���Ǿ��ִٴ� bool���� ã�Ƽ� �־���
    {
        for (int i = 0; i < itemRecipeList.Length; i++)
        {
            if (itemRecipeList[i].isClickState == true)
            {
                itemRecipeListCount = i;
                break;
            }
            else { /*PASS*/ }

        }
    }

}   //NameSpace

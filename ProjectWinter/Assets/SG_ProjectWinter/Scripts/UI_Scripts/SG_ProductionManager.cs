using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_ProductionManager : MonoBehaviour
{

    private bool isCanCraftItem;    // ������ ������ �����ʴٸ� false�� �Ǿ �Լ��� �������� ����    

    private int tempItemp001Count;
    private int tempItemp002Count;


    public void StartProduction(SG_Inventory _PlayerInven, SG_WorkStationRecipeImage _ProductionRecipe) // ���� ��ư Ŭ���� ���� �Լ�
    {

        // 23.09.22 �Ŵ����� �Ű����� ���� �ߵ�����
        //Debug.Log("�Ŵ����� �ߵ��Գ�?");
        //Debug.LogFormat("�Ű������� �� �Գ�? player -> {0}     Recipe -> {1}", _PlayerInven.name, _ProductionRecipe.itemRecipe.madeItem.itemName);
        ResetVariable();    // ���� �ʱ�ȭ ���ִ� �Լ�
        CheckMaterial001(_PlayerInven, _ProductionRecipe);


    }

    // ù��° �������� ������ �ִ���,���� �ִ��� Ȯ���ϴ� �Լ�
    private void CheckMaterial001(SG_Inventory _PlayerInven, SG_WorkStationRecipeImage _ProductionRecipe) // ù��° �������� ������ �ִ���,���� �ִ��� Ȯ��
    {
        for(int i = 0; i < _PlayerInven.slots.Length; i++)
        {

        }

    }


    private void ResetVariable()    // ���� �ʱ�ȭ ���ִ� �Լ�    �Ŵ��� �����,����� �� 2�� �ʱ�ȭ
    {
        isCanCraftItem = true;
    }
   

}

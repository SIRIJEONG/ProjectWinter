using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SG_ItemRecipe", menuName = "SG_New Item/SG_ItemRecipe")]
public class SG_ItemRecipe : ScriptableObject
{
    public SG_Item madeItem;        //������ �ϼ���
    public int madeItemCount = 1;   //�ٰ���

    public SG_Item item001;         //������ ��� 1
    public int itemCount001 = 1;    //�ʿ䰹��

    public SG_Item item002;         //������ ���2
    public int itemCount002 = 1;    //�ʿ䰹��
}

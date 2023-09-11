using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SG_New Item", menuName = "SG_New Item/SG_item")]
public class SG_Item : ScriptableObject
{



    public string itemName; //�������� �̸�
    public Sprite itemImage; //�������� �̹���

    public GameObject itemPrefab; // �������� ������

    public string weaponType; //���� ����

    public ItemType itemType;

    public enum ItemType
    {
        // ����,�����,�����,��Ÿ��
        Weapon,
        Used,
        Ingredient,
        ETC
    }

}

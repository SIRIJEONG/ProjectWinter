using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "SG_New Item", menuName = "SG_New Item/SG_item")]
public class SG_Item : ScriptableObject
{



    public string itemName; //�������� �̸�

    public Sprite itemImage; //�������� �̹���

    public GameObject itemPrefab; // �������� ������    

    public GameObject handPrefab; // �÷��̾�տ� ����� ������

    public string weaponType; //���� ����

    public int itemDamage;  // �������� ������

    public int itemHealth;  // �������� ����

    public int itemWarmth; // ���� �ö� �÷��̾� �µ�

    public int itemSatiety; // ���� �ö� �÷��̾� ������

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SG_BunkerBoxItemInIt : MonoBehaviourPun
{
    public SG_Item[] itemList;    // �پ����� List

    public GameObject itemImageObj;     // Instance ���� ������ �̹���
    private GameObject itemImageObjClone;

    private Image itemImage;    // ������ Image�� Instace�ѵڿ� �� ������ ����Ҽ� �ֵ��� �־��ٰ���

    private SG_ItemSlot itemSlotClass;  //  ������ ������ �ִ� ������ Ŭ���� sprite,item �־��ٰ���

    private int giveItemCount;  // ������ ���� � ���� �������� ������ ����

    private bool firstOpen = false;     // ó�� �������� ����ǵ��� �� bool ����
    private int tempItemListIndex;      // �����ϰ� ������ �迭�� Index ����

    void Start()
    {
        StartInIt();
    }


    private void StartInIt()    // Start�Լ����� �־��� ������
    {
        if(firstOpen == false)
        {
            firstOpen = true;
            itemSlotClass = GetComponent<SG_ItemSlot>();

            RandomGiveItemCount();      // �� ������ ���� �����ϰ� �����ִ� �Լ�
            ItemImageInIt();            // ItemImage Prefab�� Instance �ؼ� �ڽ����� �������ִ� �Լ�
            SlotItemInIt();             // �����ϰ� ���� �����۰� ������ ������ �־��ִ� �Լ�
            itemSlotClass.MoveItemSet();

        }

    }

    // �� ������ ���� �����ϰ� �����ִ� �Լ�
    private void RandomGiveItemCount()
    {
        giveItemCount = Random.Range(1, 4); //1 ~ 3 �̶�� ���� ������
        tempItemListIndex = Random.Range(0, 10);    // 0 ~ 9 ������ �� ������ 
    }

    // ItemImage Prefab�� Instance �ؼ� �ڽ����� �������ִ� �Լ�
    private void ItemImageInIt()
    {
        // �̹��� �������°��� �ڽ� ������Ʈ�� Image�� ���Ƽ� �ű�� �ִ� ���̱� ������ ���� �ִ� prefab ���� ���� �����ؼ� 
        // �길�� ItemImage�� instance�ؼ� GetParent��Ű�� �װŸ� ���ܿͼ� ����ؾ��ҰŰ��� 
        // ItemSlotClass.itemImage = ������ ������ �׵θ���

        itemImageObjClone = Instantiate(itemImageObj);
        itemImageObjClone.transform.SetParent(this.transform);



    }   // ItemImageInIt()

    // ������ ���Կ� ������ ������ ������ ī��Ʈ �־��� �Լ�
    private void SlotItemInIt()
    {
        //Debug.LogFormat("RandomIndet -> {0}",tempItemListIndex);
        //Debug.LogFormat("Item ���� -> {0}", itemList[tempItemListIndex]);
        itemSlotClass.item = itemList[tempItemListIndex];
        itemSlotClass.itemCount = giveItemCount;
    }

}

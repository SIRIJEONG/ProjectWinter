using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SG_PowerStationItemInIt : MonoBehaviourPun
{
    // ������,�︮��尡 ���ϴ� �������� �־��־���ϴµ�
    // �� ��ũ��Ʈ���� �����ҿ� �︮��尡 ���ϴ� �������� �������� �����ٰ���

    // ������ ���� ���� 3 ~ 5 �� ���̷� �������� ���ϰ� ��
    // �︮���� �����Ұ� ���� ������ = ������,���ں�ǰ,���κ�


    // [0] = ���ں�ǰ , [1] ��Ժ�ǰ , [2] ������
    public SG_Item[] itemLists; // ���ϴ� �������� �߷��� ��������

    public GameObject itemImageObj;
    private GameObject itemImageObjClone;

    private SG_Inventory inventoryClass;

    [SerializeField]
    private TextMeshProUGUI wantItemCountText;  // �������� ī��Ʈ�� üũ���� �ؽ�Ʈ Ex) 1 / 0
    private SG_ItemSlot itemSlotClass;  //  ������ ������ �ִ� ������ Ŭ���� sprite,item �־��ٰ���

    private Image itemImage;    // ������ Image�� Instace�ѵڿ� �� ������ ����Ҽ� �ֵ��� �־��ٰ���

    private Transform topParentTrans;   //�ֻ��� �θ� �˷��� Trans

    public int wantItemCount;      // ������ ������ ���ϴ� �������� Count
    private int tempItemListCount;  // ������ ������ �迭�� Index ����

    private bool isFirstOpen = false;   // ó�� �������� Instace�ǰ� ������ Count or Item Pick ���ֵ��� ���� ����
    private bool missionClear = false;  // �̼��� Ŭ���� �ȴٸ� ������ ���� �������� �������� üũ���� �Լ� �ѱ������ ����


    void Awake()
    {

    }

    void Start()
    {
        FirstInitialize();  // ������ Getcomponent �ؾ��Ұ͵��̳� ����� �̸� �����ؾ��ϴ°͵� �־��ִ� �Լ�
        if (PhotonNetwork.IsMasterClient)
        {
            FirstOpen();        // ������,�︮��� ó�� ������ �������� �������� �����ǵ��� ���ִ� �Լ�
        }
    }


    private void FirstOpen()    // ������,�︮��� ó�� ������ �������� �������� �����ǵ��� ���ִ� �Լ�
    {
        if (isFirstOpen == false)
        {
            isFirstOpen = true;
            photonView.RPC("SerchTopParentTrans", RpcTarget.All);  //�ֻ��� �θ������Ʈã�� ���� 
            RamdomItemInIt();       // �־���� �������� �������� �����ִ� �Լ�       //����
            RandomItemCountInIt();  // �־���ϴ� ������ ��ǥġ 3 ~ 5�� �����ִ� �Լ� // ����
            photonView.RPC("ApplyMissionItem", RpcTarget.Others, tempItemListCount, wantItemCount);
            photonView.RPC("ItemImageInIt", RpcTarget.All);       // �־���ϴ� �������� ������������ �ִ� ��������Ʈ�� �־��ִ� �Լ�
            photonView.RPC("ItemTextUpdateRPC", RpcTarget.All);      // ���� ���� �������� ������ �־�� �ϴ� �������� ������ �ý�Ʈ�� �����ִ� �Լ� // ����
        }
    }   // FirstOpen()


    private void FirstInitialize()
    {
        wantItemCountText = transform.Find("WantCountText").GetComponent<TextMeshProUGUI>();    // �ڽĿ�����Ʈ���� �̸����� ã�ƿ�

        itemSlotClass = GetComponent<SG_ItemSlot>();
    }

    [PunRPC]
    private void SerchTopParentTrans() //�ֻ��� �θ������Ʈã�� ����
    {
        topParentTrans = transform;

        while (topParentTrans.parent != null)
        {
            topParentTrans = topParentTrans.parent;
        }

    }


    private void RamdomItemInIt() // �־���� �������� �������� �����ִ� �Լ�
    {
        if (topParentTrans.CompareTag("PowerStation"))
        {
            tempItemListCount = Random.Range(0, itemLists.Length);
            itemSlotClass.item = itemLists[tempItemListCount];
        }
        else if (topParentTrans.CompareTag("HeliPad"))
        {
            tempItemListCount = 2;  // �︮���� ���������� ����
            itemSlotClass.item = itemLists[tempItemListCount];
        }
    }   // RamdomItemInIt()


    private void RandomItemCountInIt()  // �־���ϴ� ������ ��ǥġ 3 ~ 5�� �����ִ� �Լ�
    {
        if (topParentTrans.CompareTag("PowerStation"))
        {
            wantItemCount = Random.Range(3, 6);
        }
        else if (topParentTrans.CompareTag("HeliPad"))
        {
            wantItemCount = Random.Range(5, 8);
        }

    }   // RandomItemCountInIt()

    [PunRPC]
    public void ApplyMissionItem(int tempItemListCount_,int count)
    {
        //itemSlotClass.item.itemImage = itemImage;
        tempItemListCount = tempItemListCount_;
        wantItemCount = count;
    }

    [PunRPC]
    private void ItemImageInIt()
    {
        // �̹��� �������°��� �ڽ� ������Ʈ�� Image�� ���Ƽ� �ű�� �ִ� ���̱� ������ ���� �ִ� prefab ���� ���� �����ؼ� 
        // �길�� ItemImage�� instance�ؼ� GetParent��Ű�� �װŸ� ���ܿͼ� ����ؾ��ҰŰ��� 
        // ItemSlotClass.itemImage = ������ ������ �׵θ���

        itemImageObjClone = Instantiate(itemImageObj);
        itemImageObjClone.transform.SetParent(this.transform);
        itemImageObjClone.transform.localPosition = Vector3.zero;

        itemImage = itemImageObjClone.GetComponent<Image>();

        itemImage.sprite = itemLists[tempItemListCount].itemImage;

    }   // ItemImageInIt()

    public void ItemTextUpdate()
    {
        photonView.RPC("ItemTextUpdateRPC", RpcTarget.All);
    }

    [PunRPC]
    public void ItemTextUpdateRPC()   // ���� ���� �������� ������ �־�� �ϴ� �������� ������ �ý�Ʈ�� �����ִ� �Լ�
    {
        #region Debug
        //Debug.LogFormat("wantItemCountText == null? -> {0}", wantItemCountText == null);
        //Debug.LogFormat("itemSlotClass.itemCount -> {0}", itemSlotClass.itemCount);
        //Debug.LogFormat("wantItemCount -> {0}", wantItemCount);
        #endregion Debug
        //��Ȱ�� ����
        wantItemCountText.text = itemSlotClass.itemCount.ToString() + " / " + wantItemCount.ToString();

    }   // ItemTextUpdate()



    // ����
    // SwapManager���� Swap�� �Ǿ������� ȣ������ �Լ�
    public void CheckSucceseMission() // ������ ������ �䱸�ϴ� ��ŭ �����ϴٸ� true�� �ɰ���
    {
        if (inventoryClass == null || inventoryClass == default)
        {
            inventoryClass = transform.parent.parent.parent.GetComponent<SG_Inventory>();
        }
        else { /*PASS*/ }

        if (missionClear == false)  // �̼��� Ŭ���� ������ ���������� �Լ� ���� üũ
        {
            photonView.RPC("CheckMissionComplete", RpcTarget.All);
        }
        else { /*PASS*/ }
    }

    [PunRPC]
    public void CheckMissionComplete()
    {
        if (topParentTrans.CompareTag("PowerStation"))
        {
            if (itemSlotClass.itemCount == wantItemCount)
            {
                missionClear = true;
                inventoryClass.CheckClearPowerStation();
            }
            else { /*PASS*/ }
        }

        else if (topParentTrans.CompareTag("HeliPad"))
        {
            if (itemSlotClass.itemCount == wantItemCount)
            {
                missionClear = true;
                inventoryClass.CheckClearHeliPad();
            }
        }
    }

}   // NameSpace

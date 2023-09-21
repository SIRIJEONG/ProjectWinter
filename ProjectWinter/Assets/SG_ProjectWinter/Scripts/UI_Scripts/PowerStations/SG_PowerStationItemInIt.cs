using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SG_PowerStationItemInIt : MonoBehaviour
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

    public int wantItemCount;      // ������ ������ ���ϴ� �������� Count
    private int tempItemListCount;  // ������ ������ �迭�� Index ����

    private bool isFirstOpen = false;   // ó�� �������� Instace�ǰ� ������ Count or Item Pcik ���ֵ��� ���� ����
    private bool missionClear = false;  // �̼��� Ŭ���� �ȴٸ� ������ ���� �������� �������� üũ���� �Լ� �ѱ������ ����


    void Awake()
    {

    }

    void Start()
    {
        FirstInitialize();  // ������ Getcomponent �ؾ��Ұ͵��̳� ����� �̸� �����ؾ��ϴ°͵� �־��ִ� �Լ�
        FirstOpen();        // ������,�︮��� ó�� ������ �������� �������� �����ǵ��� ���ִ� �Լ�

    }


    private void FirstOpen()    // ������,�︮��� ó�� ������ �������� �������� �����ǵ��� ���ִ� �Լ�
    {
        if (isFirstOpen == false)
        {
            RamdomItemInIt();       // �־���� �������� �������� �����ִ� �Լ�
            RandomItemCountInIt();  // �־���ϴ� ������ ��ǥġ 3 ~ 5�� �����ִ� �Լ�
            ItemImageInIt();        // �־���ϴ� �������� ������������ �ִ� ��������Ʈ�� �־��ִ� �Լ�
            ItemTextUpdate();       // ���� ���� �������� ������ �־�� �ϴ� �������� ������ �ý�Ʈ�� �����ִ� �Լ�

        }
    }   // FirstOpen()

    private void FirstInitialize()
    {
        wantItemCountText = transform.Find("WantCountText").GetComponent<TextMeshProUGUI>();    // �ڽĿ�����Ʈ���� �̸����� ã�ƿ�

        itemSlotClass = GetComponent<SG_ItemSlot>();
    }

    private void RamdomItemInIt() // �־���� �������� �������� �����ִ� �Լ�
    {
        tempItemListCount = Random.Range(0, itemLists.Length);

        itemSlotClass.item = itemLists[tempItemListCount];
    }   // RamdomItemInIt()

    private void RandomItemCountInIt()  // �־���ϴ� ������ ��ǥġ 3 ~ 5�� �����ִ� �Լ�
    {
        wantItemCount = Random.Range(3, 6);

    }   // RandomItemCountInIt()

    private void ItemImageInIt()
    {
        // �̹��� �������°��� �ڽ� ������Ʈ�� Image�� ���Ƽ� �ű�� �ִ� ���̱� ������ ���� �ִ� prefab ���� ���� �����ؼ� 
        // �길�� ItemImage�� instance�ؼ� GetParent��Ű�� �װŸ� ���ܿͼ� ����ؾ��ҰŰ��� 
        // ItemSlotClass.itemImage = ������ ������ �׵θ���

        itemImageObjClone = Instantiate(itemImageObj);
        itemImageObjClone.transform.SetParent(this.transform);

        itemImage = itemImageObjClone.GetComponent<Image>();

        itemImage.sprite = itemSlotClass.item.itemImage;


    }   // ItemImageInIt()

    public void ItemTextUpdate()   // ���� ���� �������� ������ �־�� �ϴ� �������� ������ �ý�Ʈ�� �����ִ� �Լ�
    {
        #region Debug
        //Debug.LogFormat("wantItemCountText == null? -> {0}", wantItemCountText == null);
        //Debug.LogFormat("itemSlotClass.itemCount -> {0}", itemSlotClass.itemCount);
        //Debug.LogFormat("wantItemCount -> {0}", wantItemCount);
        #endregion Debug
        //��Ȱ�� ����
        wantItemCountText.text = itemSlotClass.itemCount.ToString() + " / " + wantItemCount.ToString();

    }   // ItemTextUpdate()

    // SwapManager���� Swap�� �Ǿ������� ȣ������ �Լ�
    public void CheckSucceseMission() // ������ ������ �䱸�ϴ� ��ŭ �����ϴٸ� true�� �ɰ���
    {
        if(inventoryClass == null || inventoryClass == default)
        {
            inventoryClass = transform.parent.parent.parent.GetComponent<SG_Inventory>();
        }

        if (missionClear == false)  // �̼��� Ŭ���� ������ ���������� �Լ� ���� üũ
        {
            if (itemSlotClass.itemCount == wantItemCount)
            {
                inventoryClass.CheckClearPowerStation();
            }
            else if (itemSlotClass.itemCount < wantItemCount)
            {
                
            }
            else { /*PASS*/ }
        }
        else { /*PASS*/ }
    }

}   // NameSpace

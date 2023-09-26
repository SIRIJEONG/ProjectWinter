using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_WorkStationControler : MonoBehaviour
{
    [SerializeField]
    private GameObject WorkStationObjs;

    private bool isOpen = false;

    private SG_PlayerActionControler playerActionClass;

    private Transform topParentTrans;

    public SG_Inventory playerInventory;   // ������ ���۽� �����۰��,���ۿ��� Ȯ���Ҷ� ����� �÷��̾��� �κ��丮 -> �̺�Ʈ�� �Ű������� ���۴븦 ������ �÷��̾��� �κ��丮�� �޾ƿ�                   

    private void Start()
    {
        FirstInIt();
    }

    private void FirstInIt()
    {
        WorkStationObjs.SetActive(false);
        playerActionClass = FindAnyObjectByType<SG_PlayerActionControler>();
        SerchTopParentTrans();  //�ֻ��� �θ� ã���ִ� �Լ�
        EventSubscriber();      // �̺�Ʈ �����ϴ� �Լ�

    }
    private void SerchTopParentTrans()  //�ֻ��� �θ� ã���ִ� �Լ�
    {
        topParentTrans = transform;

        while (topParentTrans.parent != null)
        {
            topParentTrans = topParentTrans.parent;
        }
    }

    private void EventSubscriber()  // PlayerAction Script�ӿ� Open Event ����
    {
        if (topParentTrans.CompareTag("Workstation"))
        {
            playerActionClass.WorkStationOpenEvent += WorkStationInvenController;
        }

        else if(topParentTrans.CompareTag("Kitchen"))
        {
            // ���⼭ PlauerAction�� Event�����ؾ��� �ֹ� ���� �̺�Ʈ
            playerActionClass.KitchenOpenEvent += WorkStationInvenController;
        }

        else { /*PASS*/ }
    }

    //23.09.26 �ֹ�� ���۴�� ���Լ��� ���� ����� �����۵� Ȯ������
    public void WorkStationInvenController()    // �̺�Ʈ �߻��� �ҷ��� �Լ�   
    {
        if (isOpen == false)
        {
            OpenWorkStation();
        }
        else if (isOpen == true)
        {
            CloseWorkStation();
        }
    }

    private void OpenWorkStation()
    {
        isOpen = true;
        WorkStationObjs.SetActive(true);
        playerActionClass.tossInventoryEvent += GetPlayerInventory;
    }
    private void CloseWorkStation()
    {
        isOpen = false;
        playerActionClass.tossInventoryEvent -= GetPlayerInventory;
        WorkStationObjs.SetActive(false);
    }

    private void GetPlayerInventory(SG_Inventory _playerInventory) // ���۽� �÷��̾��� �κ��丮�� ������ �Լ�
    {
        // 23.09.22 Inventory �Ű������� �߹޴°��� Ȯ��
        //Debug.Log("�Լ� �� call �Ǵ°�?");
        //Debug.LogFormat("�޾ƿ� �Ű����� �̸� -> {0}", _playerInventory.name);

        playerInventory = _playerInventory;

    }


}

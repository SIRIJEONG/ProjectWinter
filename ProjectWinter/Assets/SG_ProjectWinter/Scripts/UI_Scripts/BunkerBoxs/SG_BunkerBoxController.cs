using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SG_BunkerBoxController : MonoBehaviour
{
    [SerializeField]
    private GameObject bunkerBoxObjs;

    private bool isOpen = false;

    SG_PlayerActionControler playerActionClass;

    public int openBoxIndex;    // �ڽ� �����ؼ� �������� ����

    SG_Inventory inventoryClass;

    Coroutine eventCoroutine;

    private void Awake()
    {

    }
    private void Start()
    {
        StartInIt();
    }


    private void StartInIt()    // Start�������� ����� �Լ�
    {
        bunkerBoxObjs.SetActive(false);
        playerActionClass = FindAnyObjectByType<SG_PlayerActionControler>();
        EventSubscriber();      //�̺�Ʈ �����ϴ� �Լ�

        inventoryClass = this.transform.GetChild(0).GetChild(0).GetComponent<SG_Inventory>();

        inventoryClass.BoxIndexInIt();
        

        //Debug.LogFormat("Class Null? -> {0}, ClassName -> {1}", inventoryClass == null, inventoryClass); //�߰�����
    }

    private void EventSubscriber()
    {
        playerActionClass.BunkerBoxEvent += BunkerBoxInvenController;
    }

    public void BunkerBoxInvenController(int _OpenIndex)
    {   

        if (inventoryClass.thisBoxindex != _OpenIndex) // �÷��̾ Hit�� Ray���������ִ� slot[0].SlotCount ���ƴ϶�� return
        {
            return;
        }
        else { /*PASS*/ }

        if (isOpen == false)
        {
            OpenBunkerBox();
        }
        else if (isOpen == true)
        {
            CloseBunkerBox();
        }
        else { /*PASS*/ }
    }

    private void OpenBunkerBox()
    {
        isOpen = true;
        bunkerBoxObjs.SetActive(true);

    }
    private void CloseBunkerBox()
    {
        isOpen = false;
        bunkerBoxObjs.SetActive(false);
    }

}

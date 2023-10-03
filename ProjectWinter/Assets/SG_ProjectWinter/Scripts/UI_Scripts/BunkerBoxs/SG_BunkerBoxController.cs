using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SG_BunkerBoxController : MonoBehaviour
{
    [SerializeField]
    private GameObject bunkerBoxObjs;

    private bool isOpen = false;

    SG_PlayerActionControler playerActionClass;

    public int openBoxIndex;    // 박스 지정해서 열기위한 변수

    SG_Inventory inventoryClass;

    Coroutine eventCoroutine;

    private void Awake()
    {

    }
    private void Start()
    {
        StartInIt();
    }


    private void StartInIt()    // Start지점에서 실행될 함수
    {
        bunkerBoxObjs.SetActive(false);
        playerActionClass = FindAnyObjectByType<SG_PlayerActionControler>();
        EventSubscriber();      //이벤트 구독하는 함수

        inventoryClass = this.transform.GetChild(0).GetChild(0).GetComponent<SG_Inventory>();

        inventoryClass.BoxIndexInIt();
        

        //Debug.LogFormat("Class Null? -> {0}, ClassName -> {1}", inventoryClass == null, inventoryClass); //잘가져옴
    }

    private void EventSubscriber()
    {
        playerActionClass.BunkerBoxEvent += BunkerBoxInvenController;
    }

    public void BunkerBoxInvenController(int _OpenIndex)
    {   

        if (inventoryClass.thisBoxindex != _OpenIndex) // 플레이어가 Hit한 Ray가가지고있는 slot[0].SlotCount 가아니라면 return
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

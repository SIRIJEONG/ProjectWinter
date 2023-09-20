using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_PowerStationControler : MonoBehaviour
{

    [SerializeField]
    private GameObject poweStationObjs;

    private bool isOpen = false;
        
    SG_PlayerActionControler playerActionClass;



    private void Start()
    {
        poweStationObjs.SetActive(false);
        playerActionClass = FindAnyObjectByType<SG_PlayerActionControler>();
        playerActionClass.PowerStationInventoryEvent += PoweStationInvenController;
    }

    public void PoweStationInvenController()
    {
        //Debug.Log("이벤트로 발전기 여는 함수 조건이 잘들어와지나");
        if (isOpen == false)
        {
            OpenPoweStation();
        }
        else if (isOpen == true)
        {
            ClosePoweStation();
        }
    }

    private void OpenPoweStation()
    {
        isOpen = true;
        poweStationObjs.SetActive(true);
    }
    private void ClosePoweStation()
    {
        isOpen = false;
        poweStationObjs.SetActive(false);
    }
}

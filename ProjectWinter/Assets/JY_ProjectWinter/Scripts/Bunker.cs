using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class Bunker : MonoBehaviourPun
{
    public GameObject bunkerButton1Ui;
    public GameObject bunkerButton2Ui;

    public GameObject leftBunkerDoor;
    public GameObject rightBunkerDoor;

    private PressEKey pressEKeyOne;
    private PressEKey pressEKeyTwo;

    public bool isOpenBunker = false;
    // Start is called before the first frame update
    void Start()
    {
        pressEKeyOne = bunkerButton1Ui.GetComponent<PressEKey>();
        pressEKeyTwo = bunkerButton2Ui.GetComponent<PressEKey>();
    }

    // Update is called once per frame
    void Update()
    {
        if(pressEKeyOne.isComplete && pressEKeyTwo.isComplete && !isOpenBunker)
        {
            //문 움직이기 (두트윈 사용)
            isOpenBunker = true;
            bunkerButton1Ui.SetActive(false);
            bunkerButton2Ui.SetActive(false);

        }
    }

}

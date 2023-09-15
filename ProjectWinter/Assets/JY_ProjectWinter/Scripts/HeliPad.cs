using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public class HeliPad : MonoBehaviourPun
{
    public bool isFullGas = false;
    public AwakeNoticeCanvas awakeNoticeCanvas;
    public Player[] escapePlayers = new Player[6];


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadyHeli()
    {
        isFullGas = true;
        awakeNoticeCanvas.enabled = true;
    }

    public void TakeHeli(Player player)
    {
        if (escapePlayers[0] == null)
        {
            escapePlayers[0] = player;
        }
        else
        {
            for(int i = 0; i < escapePlayers.Length; i++)
            {
                if (escapePlayers[i] == null)
                {
                    escapePlayers[i] = player;
                    break;
                }
            }
        }
        
    }


}

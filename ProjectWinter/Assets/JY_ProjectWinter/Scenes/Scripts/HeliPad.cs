using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public class HeliPad : MonoBehaviourPun
{
    public GameObject escapePlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeHeli()
    {
        int playerActorNum = PhotonNetwork.LocalPlayer.ActorNumber;

        foreach (GameObject playerObject in GameManager.instance.playerObjects)
        {
            // �÷��̾� ������Ʈ�� PhotonView ������Ʈ�� ������
            PhotonView photonView = playerObject.GetComponent<PhotonView>();

            if (photonView != null)
            {
                // PhotonView�� ���� �÷��̾��� ������ Ȯ��
                if (photonView.IsMine)
                {
                    escapePlayer = playerObject;
                }
            }
        }
        photonView.RPC("AddEscapePalyerList", RpcTarget.All, playerActorNum);

    }

    [PunRPC]
    public void AddEscapePalyerList(int playerActorNum_)
    {
        GameManager.instance.escapePlayerList.Add(playerActorNum_);
        escapePlayer.SetActive(false);
    }


}

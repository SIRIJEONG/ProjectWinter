using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class NickName : MonoBehaviourPunCallbacks
{
    private Quaternion originalRotation; // ������ �����̼� ���� ������ ����

    public Text playerNameText; // Text UI ������Ʈ

    private void Start()
    {
        // ������ �� ������ �����̼� ���� ����
        originalRotation = transform.rotation;



        if (photonView.IsMine) // ���� �÷��̾��� ���
        {
            playerNameText.text = PhotonNetwork.LocalPlayer.NickName; // �ڽ��� �г��� ǥ��
        }
        else // ���� �÷��̾��� ���
        {
            // �ش� ���� �÷��̾��� �г����� ǥ��
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (photonView.Owner.ActorNumber == player.ActorNumber)
                {
                    playerNameText.text = player.NickName;
                    break;
                }
            }
        }

    }

    private void LateUpdate()
    {
        // ������ �����̼� ���� �����Ͽ� �׻� ���� �������� ����
        transform.rotation = originalRotation;
    }
}

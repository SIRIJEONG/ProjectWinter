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
    private Quaternion originalRotation; // 원래의 로테이션 값을 저장할 변수

    public Text playerNameText; // Text UI 컴포넌트

    private void Start()
    {
        // 시작할 때 원래의 로테이션 값을 저장
        originalRotation = transform.rotation;



        if (photonView.IsMine) // 로컬 플레이어인 경우
        {
            playerNameText.text = PhotonNetwork.LocalPlayer.NickName; // 자신의 닉네임 표시
        }
        else // 원격 플레이어인 경우
        {
            // 해당 포톤 플레이어의 닉네임을 표시
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
        // 원래의 로테이션 값을 복원하여 항상 같은 방향으로 유지
        transform.rotation = originalRotation;
    }
}

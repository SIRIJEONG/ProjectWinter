using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class HeliPad : MonoBehaviourPun
{
    public GameObject escapePlayer;
    private float time;

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
            // 플레이어 오브젝트의 PhotonView 컴포넌트를 가져옴
            PhotonView photonView = playerObject.GetComponent<PhotonView>();

            if (photonView != null)
            {
                // PhotonView가 로컬 플레이어의 것인지 확인
                if (photonView.IsMine)
                {
                    escapePlayer = playerObject;
                }
            }
        }
        photonView.RPC("AddEscapePalyerList", RpcTarget.All, playerActorNum);

        StartCoroutine(DelayedSceneChange());

    }


    private IEnumerator DelayedSceneChange()
    {
        Debug.Log("탈출한다!");

        yield return new WaitForSeconds(20f); 

        GameManager gameManager = GameManager.instance;

        gameManager.LoadGoodEnding();


    }

    [PunRPC]
    public void AddEscapePalyerList(int playerActorNum_)
    {
        GameManager.instance.escapePlayerList.Add(playerActorNum_);
        foreach (GameObject playerObject in GameManager.instance.playerObjects)
        {
            // 플레이어 오브젝트의 PhotonView 컴포넌트를 가져옴
            PhotonView photonView = playerObject.GetComponent<PhotonView>();

            if (photonView != null)
            {
                // PhotonView가 로컬 플레이어의 것인지 확인
                if (photonView.Owner.ActorNumber == playerActorNum_)
                {
                    escapePlayer = playerObject;
                }
            }
        }
        escapePlayer.SetActive(false);
    }


}
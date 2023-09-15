using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = " 1.0.0"; // ���ӹ��� 

    public TMP_Text connectionInfoText; //��Ʈ��ũ ������ ǥ���� �ؽ�Ʈ 
    public Button joinButton; // �� ���� ��ư 





    private void Start()
    {
        //���ӿ� �ʿ��� ���� ����
        PhotonNetwork.GameVersion = gameVersion;
        //������ ������ ������ ���� ���� �õ�
        PhotonNetwork.ConnectUsingSettings();
        joinButton.interactable = false;
        //���� �õ������� �ؽ�Ʈ�� ǥ�� 

    }

    public override void OnConnectedToMaster()
    {
        //�� ���� ��ư ��Ȱ��ȭ 
        joinButton.interactable = true;
        //���� ���� ǥ�� 
        connectionInfoText.text = "Online : Connected to master server succeed";

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //�� ���� ��ư ��Ȱ��ȭ 
        joinButton.interactable = false;
        //���� ���� ǥ�� 
        connectionInfoText.text = string.Format("{0}\n{1}", "offline : DisConnected : to master server\", \"Retry connect now...");
        //������ �������� ������ �õ� 
        PhotonNetwork.ConnectUsingSettings();

    }

    public void Connect()
    {
        //�ߺ� ���� �õ��� ���� ���� ���� ��ư ��� ��Ȱ��ȭ 
        joinButton.interactable = false;
        //������ ������ ���� ���̶��
        if (PhotonNetwork.IsConnected)
        {
            //�뿡 ���� 
            connectionInfoText.text = "connected to room";

            PhotonNetwork.JoinRandomRoom();

        }
        else
        {
            //������ ������ ���� ���� �ƴ϶�� ������ ������ ���� �õ�
            connectionInfoText.text = string.Format("{0}\n{1}", "offline : DisConnected : to master server", "Retry connect now...");
            //������ �������� ������ �õ� 
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //(�� ���� ����) ���� �� ������ ������ ��� �ڵ� ���� 
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //���� ���� ǥ��
        connectionInfoText.text = "Nothing to empty room , creat new room...";
        //��� �� ������ Main ���� �ε��ϰ� �� 
        PhotonNetwork.LoadLevel("HW_WaitingRoom");
    }





}




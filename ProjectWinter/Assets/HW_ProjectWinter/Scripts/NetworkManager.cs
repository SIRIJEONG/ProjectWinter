using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public AudioClip backgroundMusic; // ����������� ����� ����� Ŭ��

    private AudioSource audioSource;

    public Text severText;

    public InputField roomInput, nickNameInput;

    public AudioClip buttonClickSoundClip; // ��ư Ŭ�� ���带 ����� ����� Ŭ��


    private List<string> playerNicknames = new List<string>();

    void Awake() => Screen.SetResolution(1280  , 720, false);

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true; // ��������� ���� �ݺ� ����ϵ��� ����
        audioSource.Play();
    }

    public void Connect()
    {
        AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);
        PhotonNetwork.ConnectUsingSettings();
    } 

    public override void OnConnectedToMaster()
    {
        severText.text = "Connected Server";
        Debug.Log("�������ӿϷ�");
        PhotonNetwork.LocalPlayer.NickName = nickNameInput.text;
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause) => print("�������");

    //public void JoinLobby() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby() => print("�κ����ӿϷ�");

    public void CreateRoom() 
    {
        AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "IsReady" }; // Custom Property ����ȭ�� Ȱ��ȭ�� �Ӽ� ����
        PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 6 });
    }

    public void JoinRoom()
    {
        AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);
        PhotonNetwork.JoinRoom(roomInput.text);
    }

    public void JoinOrCreateRoom()
    {
        PhotonNetwork.JoinOrCreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 6 }, null);
    }

    public override void OnCreatedRoom() => print("�游���Ϸ�");

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("HW_WaitingRoom");
        print("�������Ϸ�");

    }

    public override void OnCreateRoomFailed(short returnCode, string message) => print("�游������");

    public override void OnJoinRoomFailed(short returnCode, string message) => print("����������");

    public override void OnJoinRandomFailed(short returnCode, string message) => print("�淣����������");
}
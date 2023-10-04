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

    public Text severText; // ���� ���¸� ǥ���� �ؽ�Ʈ UI

    public InputField roomInput, nickNameInput; // �� �̸��� �г����� �Է¹��� ��ǲ �ʵ� UI

    public AudioClip buttonClickSoundClip; // ��ư Ŭ�� ���带 ����� ����� Ŭ��

    private List<string> playerNicknames = new List<string>(); // �÷��̾� �г����� ������ ����Ʈ

    // ���� ���� �� ȣ��Ǵ� �Լ���, ȭ�� �ػ󵵸� �����մϴ�.
    void Awake() => Screen.SetResolution(1280, 720, false);

    // ���� ������Ʈ�� Ȱ��ȭ�� �� ȣ��Ǵ� �Լ���, ����� ������ �ʱ�ȭ�ϰ� ��������� ����մϴ�.
    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true; // ��������� ���� �ݺ� ����ϵ��� ����
        audioSource.Play();
    }

    // Connect �޼���: ������ �����ϴ� �Լ���, ��ư Ŭ�� �� ȣ��˴ϴ�.
    public void Connect()
    {
        AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);
        PhotonNetwork.ConnectUsingSettings();
    }

    // OnConnectedToMaster �޼���: ������ ������ ����Ǿ��� �� ȣ��Ǵ� �ݹ� �Լ���, ���� ���� ���¸� ǥ���մϴ�.
    public override void OnConnectedToMaster()
    {
        severText.text = "Connected Server";
        Debug.Log("�������ӿϷ�");
        PhotonNetwork.LocalPlayer.NickName = nickNameInput.text; // ���� �÷��̾��� �г��� ����
    }

    // Disconnect �޼���: �������� ������ ���� �Լ���, ��ư Ŭ�� �� ȣ��˴ϴ�.
    public void Disconnect() => PhotonNetwork.Disconnect();

    // OnDisconnected �޼���: ���� ������ ������ �� ȣ��Ǵ� �ݹ� �Լ���, ���� ���� ���¸� ǥ���մϴ�.
    public override void OnDisconnected(DisconnectCause cause) => print("�������");

    // OnJoinedLobby �޼���: �κ� �������� �� ȣ��Ǵ� �ݹ� �Լ���, �κ� ���� ���¸� ǥ���մϴ�.
    public override void OnJoinedLobby() => print("�κ����ӿϷ�");

    // CreateRoom �޼���: ���� �����ϴ� �Լ���, ��ư Ŭ�� �� ȣ��˴ϴ�.
    public void CreateRoom()
    {
        AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "IsReady" }; // Custom Property ����ȭ�� Ȱ��ȭ�� �Ӽ� ���� (���� �̷��� ������� ���� �Ӽ��� �κ� �ִ� �ٸ� �÷��̾�� �����ְ� ����ȭ ���� ���θ� ����)
        PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 6 });
    }

    // JoinRoom �޼���: �濡 �����ϴ� �Լ���, ��ư Ŭ�� �� ȣ��˴ϴ�.
    public void JoinRoom()
    {
        AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);
        PhotonNetwork.JoinRoom(roomInput.text);
    }

    // JoinOrCreateRoom �޼���: �濡 �����ϰų� ���� �����ϴ� �Լ���, ��ư Ŭ�� �� ȣ��˴ϴ�.
    public void JoinOrCreateRoom()
    {
        PhotonNetwork.JoinOrCreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 6 }, null);
    }

    // OnCreatedRoom �޼���: ���� ���������� �������� �� ȣ��Ǵ� �ݹ� �Լ���, ���� �Ϸ� ���¸� ǥ���մϴ�.
    public override void OnCreatedRoom() => print("�游���Ϸ�");

    // OnJoinedRoom �޼���: �濡 ���������� �������� �� ȣ��Ǵ� �ݹ� �Լ���, �� ���� ���¸� ǥ���ϰ� ���� ������ ��ȯ�մϴ�.
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("HW_WaitingRoom");
        print("�������Ϸ�");
    }

    // OnCreateRoomFailed �޼���: �� ������ �������� �� ȣ��Ǵ� �ݹ� �Լ���, �� ����� ���� ���¸� ǥ���մϴ�.
    public override void OnCreateRoomFailed(short returnCode, string message) => print("�游������");

    // OnJoinRoomFailed �޼���: �� ������ �������� �� ȣ��Ǵ� �ݹ� �Լ���, �� ���� ���� ���¸� ǥ���մϴ�.
    public override void OnJoinRoomFailed(short returnCode, string message) => print("����������");

    // OnJoinRandomFailed �޼���: ���� �� ������ �������� �� ȣ��Ǵ� �ݹ� �Լ���, ���� �� ���� ���� ���¸� ǥ���մϴ�.
    public override void OnJoinRandomFailed(short returnCode, string message) => print("�淣����������");
}

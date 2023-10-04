using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public AudioClip backgroundMusic; // 배경음악으로 사용할 오디오 클립

    private AudioSource audioSource;

    public Text severText; // 서버 상태를 표시할 텍스트 UI

    public InputField roomInput, nickNameInput; // 방 이름과 닉네임을 입력받을 인풋 필드 UI

    public AudioClip buttonClickSoundClip; // 버튼 클릭 사운드를 재생할 오디오 클립

    private List<string> playerNicknames = new List<string>(); // 플레이어 닉네임을 저장할 리스트

    // 게임 시작 시 호출되는 함수로, 화면 해상도를 설정합니다.
    void Awake() => Screen.SetResolution(1280, 720, false);

    // 게임 오브젝트가 활성화될 때 호출되는 함수로, 오디오 설정을 초기화하고 배경음악을 재생합니다.
    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true; // 배경음악을 무한 반복 재생하도록 설정
        audioSource.Play();
    }

    // Connect 메서드: 서버에 연결하는 함수로, 버튼 클릭 시 호출됩니다.
    public void Connect()
    {
        AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);
        PhotonNetwork.ConnectUsingSettings();
    }

    // OnConnectedToMaster 메서드: 마스터 서버에 연결되었을 때 호출되는 콜백 함수로, 서버 연결 상태를 표시합니다.
    public override void OnConnectedToMaster()
    {
        severText.text = "Connected Server";
        Debug.Log("서버접속완료");
        PhotonNetwork.LocalPlayer.NickName = nickNameInput.text; // 로컬 플레이어의 닉네임 설정
    }

    // Disconnect 메서드: 서버에서 연결을 끊는 함수로, 버튼 클릭 시 호출됩니다.
    public void Disconnect() => PhotonNetwork.Disconnect();

    // OnDisconnected 메서드: 서버 연결이 끊겼을 때 호출되는 콜백 함수로, 연결 끊김 상태를 표시합니다.
    public override void OnDisconnected(DisconnectCause cause) => print("연결끊김");

    // OnJoinedLobby 메서드: 로비에 접속했을 때 호출되는 콜백 함수로, 로비 접속 상태를 표시합니다.
    public override void OnJoinedLobby() => print("로비접속완료");

    // CreateRoom 메서드: 방을 생성하는 함수로, 버튼 클릭 시 호출됩니다.
    public void CreateRoom()
    {
        AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "IsReady" }; // Custom Property 동기화를 활성화할 속성 지정 (방의 이러한 사용자의 정의 속성을 로비에 있는 다른 플레이어에게 보여주고 동기화 할지 여부를 설정)
        PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 6 });
    }

    // JoinRoom 메서드: 방에 참가하는 함수로, 버튼 클릭 시 호출됩니다.
    public void JoinRoom()
    {
        AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);
        PhotonNetwork.JoinRoom(roomInput.text);
    }

    // JoinOrCreateRoom 메서드: 방에 참가하거나 방을 생성하는 함수로, 버튼 클릭 시 호출됩니다.
    public void JoinOrCreateRoom()
    {
        PhotonNetwork.JoinOrCreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 6 }, null);
    }

    // OnCreatedRoom 메서드: 방을 성공적으로 생성했을 때 호출되는 콜백 함수로, 생성 완료 상태를 표시합니다.
    public override void OnCreatedRoom() => print("방만들기완료");

    // OnJoinedRoom 메서드: 방에 성공적으로 참가했을 때 호출되는 콜백 함수로, 방 참가 상태를 표시하고 다음 씬으로 전환합니다.
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("HW_WaitingRoom");
        print("방참가완료");
    }

    // OnCreateRoomFailed 메서드: 방 생성에 실패했을 때 호출되는 콜백 함수로, 방 만들기 실패 상태를 표시합니다.
    public override void OnCreateRoomFailed(short returnCode, string message) => print("방만들기실패");

    // OnJoinRoomFailed 메서드: 방 참가에 실패했을 때 호출되는 콜백 함수로, 방 참가 실패 상태를 표시합니다.
    public override void OnJoinRoomFailed(short returnCode, string message) => print("방참가실패");

    // OnJoinRandomFailed 메서드: 랜덤 방 참가에 실패했을 때 호출되는 콜백 함수로, 랜덤 방 참가 실패 상태를 표시합니다.
    public override void OnJoinRandomFailed(short returnCode, string message) => print("방랜덤참가실패");
}

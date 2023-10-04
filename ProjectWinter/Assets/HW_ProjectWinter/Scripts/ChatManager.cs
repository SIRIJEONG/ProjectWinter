using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections;

public class ChatManager : MonoBehaviourPunCallbacks
{
    public Text chatText;               // 채팅 메시지를 표시할 텍스트 UI
    public InputField chatInput;        // 플레이어의 채팅 입력을 받을 인풋 필드
    private string playerName;          // 플레이어의 닉네임
    public Scrollbar scrollbar;         // 스크롤바

    public PlayerController playerController; // PlayerController 스크립트에 접근하기 위한 변수

    private bool inputFieldActive = false; // 인풋 필드의 활성 상태를 추적하는 변수

    private void Start()
    {
        playerName = PhotonNetwork.LocalPlayer.NickName;
        playerController = FindObjectOfType<PlayerController>(); // PlayerController 스크립트를 찾아 playerController 변수에 할당
    }

    private void Update()
    {
        // 인풋 필드가 포커스를 가지고 있고, 특정 조건을 만족할 때 PlayerController의 doSomething을 true로 설정
        if (chatInput.isFocused && !playerController.doSomething)
        {
            playerController.doSomething = true;
        }

        // Return 키가 눌렸고, 입력된 텍스트가 비어 있지 않은 경우
        if (Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrEmpty(chatInput.text))
        {
            string message = playerName + ": " + chatInput.text; // 플레이어 이름과 입력된 텍스트를 합쳐 메시지 생성
            SendChatMessage(message); // 메시지를 전송
            chatInput.text = ""; // 입력 필드 초기화

            ActivateInputField(); // 입력 필드 활성화 함수 호출
        }

        // 인풋 필드가 활성화되어 있고 마우스 왼쪽 버튼이 클릭된 경우
        if (inputFieldActive && Input.GetMouseButton(0))
        {
            if (!chatInput.isFocused)
            {
                DeactivateInputField(); // 입력 필드를 비활성화
            }
        }
    }

    // 메시지를 전송하는 함수
    private void SendChatMessage(string message)
    {
        photonView.RPC("ReceiveChatMessage", RpcTarget.All, message); // 모든 플레이어에게 메시지를 전송하는 RPC 호출
    }

    // RPC를 통해 메시지를 수신하고 화면에 표시하는 함수
    [PunRPC]
    private void ReceiveChatMessage(string message)
    {
        chatText.text += "\n" + message; // 기존 채팅 텍스트에 새로운 메시지 추가

        // 스크롤바 값을 최하단으로 조정하여 아래로 스크롤
        scrollbar.value = 0f;
    }

    // 인풋 필드를 활성화하는 함수
    private void ActivateInputField()
    {
        chatInput.ActivateInputField(); // 인풋 필드 활성화
        inputFieldActive = true; // 활성 상태 설정
        if (playerController != null)
        {
            playerController.doSomething = true; // PlayerController의 doSomething을 true로 설정
        }
    }

    // 다른 곳을 클릭했을 때 인풋 필드를 비활성화하는 함수
    public void DeactivateInputField()
    {
        chatInput.DeactivateInputField(); // 인풋 필드 비활성화
        inputFieldActive = false; // 비활성 상태 설정
        playerController.doSomething = false; // PlayerController의 doSomething을 false로 설정
    }
}

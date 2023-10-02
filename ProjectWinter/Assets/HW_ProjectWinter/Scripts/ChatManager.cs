using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections;

public class ChatManager : MonoBehaviourPunCallbacks
{
    public Text chatText;
    public InputField chatInput;
    private string playerName;
    public Scrollbar scrollbar; // 스크롤바

    public PlayerController playerController; // PlayerController 스크립트에 접근하기 위한 변수

    private bool inputFieldActive = false; // 인풋 필드의 활성 상태를 추적하는 변수

    private void Start()
    {
        playerName = PhotonNetwork.LocalPlayer.NickName;
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if(chatInput.isFocused && !playerController.doSomething)
        {
            playerController.doSomething = true;
        }
        

        //if (Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrEmpty(chatInput.text))
        //{
        //    string message = playerName + ": " + chatInput.text;
        //    SendChatMessage(message);
        //    chatInput.text = "";

        //    chatInput.ActivateInputField();

        //}

        // 인풋 필드가 활성화되지 않았고, Return 키가 눌렸을 때
        if (Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrEmpty(chatInput.text))
        {

            string message = playerName + ": " + chatInput.text;
            SendChatMessage(message);
            chatInput.text = "";

            ActivateInputField();

          
        }

        
        if(inputFieldActive && Input.GetMouseButton(0))
        {
            if(!chatInput.isFocused)
            {
                DeactivateInputField();
            }       
        }

    }

    private void SendChatMessage(string message)
    {
        photonView.RPC("ReceiveChatMessage", RpcTarget.All, message);
    }

    [PunRPC]
    private void ReceiveChatMessage(string message)
    {
        chatText.text += "\n" + message;

        // 스크롤바 값을 최대로 조정하여 아래로 스크롤
        scrollbar.value = 0f;

    }

    // 인풋 필드를 활성화하는 함수
    private void ActivateInputField()
    {
        Debug.Log("된다");
        chatInput.ActivateInputField();
        inputFieldActive = true;
        if(playerController != null)
        {
            playerController.doSomething = true;
        }

    }

    // 다른 곳을 클릭했을 때 인풋 필드를 비활성화하는 함수
    public void DeactivateInputField()
    {
        Debug.Log("안된다");

        chatInput.DeactivateInputField();
        inputFieldActive = false;
        playerController.doSomething = false;
    }


}

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
    public Scrollbar scrollbar; // ��ũ�ѹ�

    public PlayerController playerController; // PlayerController ��ũ��Ʈ�� �����ϱ� ���� ����

    private bool inputFieldActive = false; // ��ǲ �ʵ��� Ȱ�� ���¸� �����ϴ� ����

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

        // ��ǲ �ʵ尡 Ȱ��ȭ���� �ʾҰ�, Return Ű�� ������ ��
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

        // ��ũ�ѹ� ���� �ִ�� �����Ͽ� �Ʒ��� ��ũ��
        scrollbar.value = 0f;

    }

    // ��ǲ �ʵ带 Ȱ��ȭ�ϴ� �Լ�
    private void ActivateInputField()
    {
        Debug.Log("�ȴ�");
        chatInput.ActivateInputField();
        inputFieldActive = true;
        if(playerController != null)
        {
            playerController.doSomething = true;
        }

    }

    // �ٸ� ���� Ŭ������ �� ��ǲ �ʵ带 ��Ȱ��ȭ�ϴ� �Լ�
    public void DeactivateInputField()
    {
        Debug.Log("�ȵȴ�");

        chatInput.DeactivateInputField();
        inputFieldActive = false;
        playerController.doSomething = false;
    }


}

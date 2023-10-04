using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections;

public class ChatManager : MonoBehaviourPunCallbacks
{
    public Text chatText;               // ä�� �޽����� ǥ���� �ؽ�Ʈ UI
    public InputField chatInput;        // �÷��̾��� ä�� �Է��� ���� ��ǲ �ʵ�
    private string playerName;          // �÷��̾��� �г���
    public Scrollbar scrollbar;         // ��ũ�ѹ�

    public PlayerController playerController; // PlayerController ��ũ��Ʈ�� �����ϱ� ���� ����

    private bool inputFieldActive = false; // ��ǲ �ʵ��� Ȱ�� ���¸� �����ϴ� ����

    private void Start()
    {
        playerName = PhotonNetwork.LocalPlayer.NickName;
        playerController = FindObjectOfType<PlayerController>(); // PlayerController ��ũ��Ʈ�� ã�� playerController ������ �Ҵ�
    }

    private void Update()
    {
        // ��ǲ �ʵ尡 ��Ŀ���� ������ �ְ�, Ư�� ������ ������ �� PlayerController�� doSomething�� true�� ����
        if (chatInput.isFocused && !playerController.doSomething)
        {
            playerController.doSomething = true;
        }

        // Return Ű�� ���Ȱ�, �Էµ� �ؽ�Ʈ�� ��� ���� ���� ���
        if (Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrEmpty(chatInput.text))
        {
            string message = playerName + ": " + chatInput.text; // �÷��̾� �̸��� �Էµ� �ؽ�Ʈ�� ���� �޽��� ����
            SendChatMessage(message); // �޽����� ����
            chatInput.text = ""; // �Է� �ʵ� �ʱ�ȭ

            ActivateInputField(); // �Է� �ʵ� Ȱ��ȭ �Լ� ȣ��
        }

        // ��ǲ �ʵ尡 Ȱ��ȭ�Ǿ� �ְ� ���콺 ���� ��ư�� Ŭ���� ���
        if (inputFieldActive && Input.GetMouseButton(0))
        {
            if (!chatInput.isFocused)
            {
                DeactivateInputField(); // �Է� �ʵ带 ��Ȱ��ȭ
            }
        }
    }

    // �޽����� �����ϴ� �Լ�
    private void SendChatMessage(string message)
    {
        photonView.RPC("ReceiveChatMessage", RpcTarget.All, message); // ��� �÷��̾�� �޽����� �����ϴ� RPC ȣ��
    }

    // RPC�� ���� �޽����� �����ϰ� ȭ�鿡 ǥ���ϴ� �Լ�
    [PunRPC]
    private void ReceiveChatMessage(string message)
    {
        chatText.text += "\n" + message; // ���� ä�� �ؽ�Ʈ�� ���ο� �޽��� �߰�

        // ��ũ�ѹ� ���� ���ϴ����� �����Ͽ� �Ʒ��� ��ũ��
        scrollbar.value = 0f;
    }

    // ��ǲ �ʵ带 Ȱ��ȭ�ϴ� �Լ�
    private void ActivateInputField()
    {
        chatInput.ActivateInputField(); // ��ǲ �ʵ� Ȱ��ȭ
        inputFieldActive = true; // Ȱ�� ���� ����
        if (playerController != null)
        {
            playerController.doSomething = true; // PlayerController�� doSomething�� true�� ����
        }
    }

    // �ٸ� ���� Ŭ������ �� ��ǲ �ʵ带 ��Ȱ��ȭ�ϴ� �Լ�
    public void DeactivateInputField()
    {
        chatInput.DeactivateInputField(); // ��ǲ �ʵ� ��Ȱ��ȭ
        inputFieldActive = false; // ��Ȱ�� ���� ����
        playerController.doSomething = false; // PlayerController�� doSomething�� false�� ����
    }
}

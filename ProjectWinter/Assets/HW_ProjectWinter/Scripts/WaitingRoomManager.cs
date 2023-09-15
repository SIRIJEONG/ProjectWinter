using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class WaitingRoomManager : MonoBehaviourPunCallbacks
{
    public Text nicknameBoxOne;
    public Text nicknameBoxOne2;
    public Text nicknameBoxOne3;
    public Text nicknameBoxOne4;
    public Text nicknameBoxOne5;
    public Text nicknameBoxOne6;
    public Text userCount;

    public Image[] targetImages; // �������� ������ ��� �̹��� �迭

    //public Image targetImage; // �������� ������ ��� �̹���
    //public Image targetImage2; // �������� ������ ��� �̹���
    //public Image targetImage3; // �������� ������ ��� �̹���
    //public Image targetImage4; // �������� ������ ��� �̹���
    //public Image targetImage5; // �������� ������ ��� �̹���
    //public Image targetImage6; // �������� ������ ��� �̹���

    private Color originalColor; // �ʱ� �̹��� ����
    private Color transparentColor; // Ŭ�� �� ���� 

    private List<Text> nicknameBoxes = new List<Text>();

    public bool readyButton = true;

    // Start is called before the first frame update
    void Start()
    {
        // �ؽ�Ʈ UI�� ����Ʈ�� �߰�
        nicknameBoxes.Add(nicknameBoxOne);
        nicknameBoxes.Add(nicknameBoxOne2);
        nicknameBoxes.Add(nicknameBoxOne3);
        nicknameBoxes.Add(nicknameBoxOne4);
        nicknameBoxes.Add(nicknameBoxOne5);
        nicknameBoxes.Add(nicknameBoxOne6);

        // �÷��̾� ���� �� ����� ���õ� �̺�Ʈ �ڵ鷯 ���
        PhotonNetwork.AddCallbackTarget(this);


        //// �ʱ� �̹��� ������ ����
        originalColor = targetImages[0].color;

        // Ŭ���� ���� 
        transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 170f);
        UpdateNicknameText();
        UpdateUserCount();


    }

    // �ٸ� �÷��̾ �濡 ������ �� ȣ��Ǵ� �ݹ�
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // ���ο� �÷��̾ �濡 �����ϸ� �̺�Ʈ�� ȣ��˴ϴ�.
        // �̶� ���ο� �÷��̾��� �г����� Text UI�� �Ҵ��Ͽ� ǥ���մϴ�.
        UpdateNicknameText();
        UpdateUserCount();
    }

    // �ٸ� �÷��̾ �濡�� ������ �� ȣ��Ǵ� �ݹ�
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // �ٸ� �÷��̾ �濡�� �����ϸ� �̺�Ʈ�� ȣ��˴ϴ�.
        // �̶� �ش� �÷��̾��� Text UI�� �ʱ�ȭ�Ͽ� ���ϴ�.
        ResetNicknameText(otherPlayer.NickName);
        UpdateUserCount();
        UpdateNicknameText();
        AllResetNicknameText();
    }

    // Text UI�� �÷��̾� �г��� ǥ�� ������Ʈ �޼���
    void UpdateNicknameText()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (i < nicknameBoxes.Count)
            {
                nicknameBoxes[i].text = PhotonNetwork.PlayerList[i].NickName;
            }
        }
    }

    // Text UI�� �ʱ�ȭ�ϴ� �޼���
    void ResetNicknameText(string playerNickname)
    {

        foreach (var text in nicknameBoxes)
        {
            if (text.text == playerNickname)
            {
                text.text = "�˻� ��....."; // �ش� �÷��̾��� Text UI�� ���ϴ�.
            }
        }
    }


    void AllResetNicknameText()
    {
        for (int i = PhotonNetwork.PlayerList.Length; i < 6; i++)
        {
            nicknameBoxes[i].text = "�˻� ��.....";
            targetImages[i].color = originalColor;
        }
    }

    public void UpdateUserCount()
    {
        userCount.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "/6";
    }

    // Update is called once per frame
    void Update()
    {

    }

    // public void GetReadyButton()
    // {

    //     // ���� �÷��̾��� �Է� �Ǵ� � ���ǿ� ���� �������� ���� ���� 
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         // ���� ������ �������� ����
    //         photonView.RPC("SetImageColor", RpcTarget.AllBuffered, transparentColor);
    //     }
    //     SetImageColor(transparentColor);

    //     Ŭ�� �� �̹��� ����
    //     photonView.RPC("SetImageColor", RpcTarget.All);



    // }
    // RPC�� ���� �̹����� ������ �����ϴ� �޼���
    //[PunRPC]
    // public void SetImageColor(/*Color color*/)
    // {
    //     for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
    //     {
    //         if (i < nicknameBoxes.Count)
    //         {

    //             if (PhotonNetwork.PlayerList[i].NickName == nicknameBoxes[i].text)
    //             {
    //                 if (i == 0)
    //                 {
    //                     targetImage.color = transparentColor;
    //                 }
    //                 else if (i == 1)
    //                 {
    //                     targetImage2.color = transparentColor;
    //                 }
    //                 else if (i == 2)
    //                 {
    //                     targetImage3.color = transparentColor;
    //                 }
    //                 else if (i == 3)
    //                 {
    //                     targetImage4.color = transparentColor;
    //                 }
    //                 else if (i == 4)
    //                 {
    //                     targetImage5.color = transparentColor;
    //                 }
    //                 else if (i == 5)
    //                 {
    //                     targetImage6.color = transparentColor;
    //                 }
    //             }
    //         }
    //     }
    // }










    public void GetReadyButton()
    {

        string localPlayerNickname = PhotonNetwork.LocalPlayer.NickName;
        int playerIndex = GetPlayerIndex(localPlayerNickname);

        if (playerIndex >= 0 && playerIndex < targetImages.Length)
        {
            if (readyButton == true)
            {
                // Ŭ�� �� �̹��� ����
                photonView.RPC("SetImageColor", RpcTarget.All, playerIndex);
                readyButton = false;
            }
            else if (readyButton == false)
            {
                photonView.RPC("UnSetImageColor", RpcTarget.All, playerIndex);
                readyButton = true;
            }
        }

    }



    private int GetPlayerIndex(string playerNickname)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].NickName == playerNickname)
            {
                return i;
            }
        }
        return -1;
    }

    // RPC�� ���� �̹����� ������ �����ϴ� �޼���
    [PunRPC]
    public void SetImageColor(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < targetImages.Length)
        {
            Image targetImageToUpdate = targetImages[playerIndex];

            if (targetImageToUpdate != null)
            {
                targetImageToUpdate.color = transparentColor;
            }
        }
    }

    // RPC�� ���� �̹����� ������ �����ϴ� �޼���
    [PunRPC]
    public void UnSetImageColor(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < targetImages.Length)
        {
            Image targetImageToUpdate = targetImages[playerIndex];

            if (targetImageToUpdate != null)
            {
                targetImageToUpdate.color = originalColor;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
//using UnityEditor.XR;

public class WaitingRoomManager : MonoBehaviourPunCallbacks
{

    public Slider masterVolumeSlider; // ������ ������ �����ϴ� �����̴�
    private float initialMasterVolume; // �ʱ� ������ ���� ����

    public AudioClip buttonClickSoundClip; // ��ư Ŭ�� ���带 ����� ����� Ŭ��


    public GameObject optionUI; // NeighbourUI �������� �� �� ������ ���� ui


    public Text nicknameBoxOne;
    public Text nicknameBoxOne2;
    public Text nicknameBoxOne3;
    public Text nicknameBoxOne4;
    public Text nicknameBoxOne5;
    public Text nicknameBoxOne6;
    public Text userCount;

    public Image[] targetImages; // �������� ������ ��� �̹��� �迭


    private Color originalColor; // �ʱ� �̹��� ����
    private Color transparentColor; // Ŭ�� �� ���� 

    private List<Text> nicknameBoxes = new List<Text>();

    // ������ �߰��Ͽ� ��� �÷��̾��� readyButton ���¸� �����մϴ�.
    private Dictionary<string, bool> playerReadyStates = new Dictionary<string, bool>();

    public bool readyButton = true;


    // Start is called before the first frame updated
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;


        // �ʱ� ������ ������ ����
        initialMasterVolume = AudioListener.volume;

        // �����̴��� ���� �ʱ� ������ �������� ����
        masterVolumeSlider.value = initialMasterVolume;

        // �����̴��� ���� ����� ������ �̺�Ʈ �ڵ鷯 ȣ��
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeSliderChanged);


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

    public void SetPlayerReady(bool isReady)
    {
        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
        customProperties["IsReady"] = isReady;
        PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
    }


    public bool AreAllPlayersReady()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            object isReady;
            if (player.CustomProperties.TryGetValue("IsReady", out isReady))
            {
                if (!(bool)isReady)
                {
                    return false; // �ϳ� �̻��� �÷��̾ ���� �غ����� ����
                }
            }
            else
            {
                return false; // �÷��̾��� �غ� ���� Custom Property�� ���ų� ���� ������ �� ����
            }
        }
        return true; // ��� �÷��̾ �غ� �Ϸ�
    }



    // �ٸ� �÷��̾ �濡 ������ �� ȣ��Ǵ� �ݹ�
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // ���ο� �÷��̾ �濡 �����ϸ� �̺�Ʈ�� ȣ��˴ϴ�.
        // �̶� ���ο� �÷��̾��� �г����� Text UI�� �Ҵ��Ͽ� ǥ���մϴ�.
        playerReadyStates[newPlayer.NickName] = false;

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





    //public void SetPlayerReadyState(string playerNickname, bool isReady)
    //{
    //    playerReadyStates[playerNickname] = isReady;
    //}

    //public bool AreAllPlayersReady()
    //{
    //    foreach (var rc in playerReadyStates)
    //    {
    //        if (rc.Value)
    //        {
    //            return false; // �ϳ� �̻��� �÷��̾ ���� �غ����� ����
    //        }
    //    }
    //    return true; // ��� �÷��̾ �غ� �Ϸ�
    //}



    // ���� ���� ��ư�� ���� �� ȣ��Ǵ� �޼���
    public void StartGame()
    {
        if (AreAllPlayersReady())
        {
            // ��� �÷��̾ �غ� ������ ���, �� ��ȯ �Ǵ� ���� ���� ������ �����մϴ�.
            // ���� ���, ���� ��ȯ�Ϸ��� ������ ���� ����մϴ�.
            AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);
            Debug.Log("���� ���� ���� ����: ��� �÷��̾ �غ� �����Դϴ�.");
            PhotonNetwork.LoadLevel("HW_LoadingScene");
        }
        else
        {
            Debug.Log("���� ���� ���� ������: ���� ��� �÷��̾ �غ� ���°� �ƴմϴ�.");
            // ���� ��� �÷��̾ �غ� ���°� �ƴ��� �޽����� ǥ���ϰų� �ٸ� ������ �����մϴ�.
        }
    }






    // Update is called once per frame
    void Update()
    {

    }

    public void GetReadyButton()
    {

        string localPlayerNickname = PhotonNetwork.LocalPlayer.NickName;
        int playerIndex = GetPlayerIndex(localPlayerNickname);

        if (playerIndex >= 0 && playerIndex < targetImages.Length)
        {
            if (readyButton == true)
            {
                // Ŭ�� �� �̹��� ����
                photonView.RPC("SetImageColor", RpcTarget.AllBuffered, playerIndex);
                readyButton = false;
                AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);
            }
            else if (readyButton == false)
            {
                photonView.RPC("UnSetImageColor", RpcTarget.AllBuffered, playerIndex);
                readyButton = true;
                AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);
            }
        }

        SetPlayerReady(!readyButton);

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



    public void LeftGame()
    {
        PhotonNetwork.Disconnect(); // �������� ������ ����
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        // �������� ������ �������� �κ� ������
        readyButton = true;
        AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);
        PhotonNetwork.LoadLevel("HW_Lobby");
        PhotonNetwork.JoinLobby();
    }



    public void UIScale()
    {
        AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);

        Vector3 uiImageScale = optionUI.transform.localScale;

        Vector3 newScale = new Vector3(0.001f, 0.001f, 0.001f);

        optionUI.transform.localScale = newScale;
    }

    public void UIScale2()
    {
        AudioSource.PlayClipAtPoint(buttonClickSoundClip, Camera.main.transform.position);

        Vector3 uiImageScale = optionUI.transform.localScale;

        Vector3 newScale = new Vector3(5f, 5f, 5f);

        optionUI.transform.localScale = newScale;
    }


    // �����̴� ���� ����� �� ȣ��Ǵ� �޼���
    void OnMasterVolumeSliderChanged(float value)
    {
        // �����̴� ���� AudioListener.volume�� �����Ͽ� ������ ���� ����
        AudioListener.volume = value;
    }

}

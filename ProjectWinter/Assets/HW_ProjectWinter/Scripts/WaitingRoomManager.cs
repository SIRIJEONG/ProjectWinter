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

    public Image[] targetImages; // 불투명도를 조절할 대상 이미지 배열

    //public Image targetImage; // 불투명도를 조절할 대상 이미지
    //public Image targetImage2; // 불투명도를 조절할 대상 이미지
    //public Image targetImage3; // 불투명도를 조절할 대상 이미지
    //public Image targetImage4; // 불투명도를 조절할 대상 이미지
    //public Image targetImage5; // 불투명도를 조절할 대상 이미지
    //public Image targetImage6; // 불투명도를 조절할 대상 이미지

    private Color originalColor; // 초기 이미지 색상
    private Color transparentColor; // 클릭 후 색상 

    private List<Text> nicknameBoxes = new List<Text>();

    public bool readyButton = true;

    // Start is called before the first frame update
    void Start()
    {
        // 텍스트 UI를 리스트에 추가
        nicknameBoxes.Add(nicknameBoxOne);
        nicknameBoxes.Add(nicknameBoxOne2);
        nicknameBoxes.Add(nicknameBoxOne3);
        nicknameBoxes.Add(nicknameBoxOne4);
        nicknameBoxes.Add(nicknameBoxOne5);
        nicknameBoxes.Add(nicknameBoxOne6);

        // 플레이어 입장 및 퇴장과 관련된 이벤트 핸들러 등록
        PhotonNetwork.AddCallbackTarget(this);


        //// 초기 이미지 색상을 저장
        originalColor = targetImages[0].color;

        // 클릭후 색상 
        transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 170f);
        UpdateNicknameText();
        UpdateUserCount();


    }

    // 다른 플레이어가 방에 입장할 때 호출되는 콜백
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 새로운 플레이어가 방에 입장하면 이벤트가 호출됩니다.
        // 이때 새로운 플레이어의 닉네임을 Text UI에 할당하여 표시합니다.
        UpdateNicknameText();
        UpdateUserCount();
    }

    // 다른 플레이어가 방에서 퇴장할 때 호출되는 콜백
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // 다른 플레이어가 방에서 퇴장하면 이벤트가 호출됩니다.
        // 이때 해당 플레이어의 Text UI를 초기화하여 비웁니다.
        ResetNicknameText(otherPlayer.NickName);
        UpdateUserCount();
        UpdateNicknameText();
        AllResetNicknameText();
    }

    // Text UI에 플레이어 닉네임 표시 업데이트 메서드
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

    // Text UI를 초기화하는 메서드
    void ResetNicknameText(string playerNickname)
    {

        foreach (var text in nicknameBoxes)
        {
            if (text.text == playerNickname)
            {
                text.text = "검색 중....."; // 해당 플레이어의 Text UI를 비웁니다.
            }
        }
    }


    void AllResetNicknameText()
    {
        for (int i = PhotonNetwork.PlayerList.Length; i < 6; i++)
        {
            nicknameBoxes[i].text = "검색 중.....";
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

    //     // 로컬 플레이어의 입력 또는 어떤 조건에 따라 불투명도를 변경 가능 
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         // 완전 투명한 색상으로 변경
    //         photonView.RPC("SetImageColor", RpcTarget.AllBuffered, transparentColor);
    //     }
    //     SetImageColor(transparentColor);

    //     클릭 후 이미지 색상
    //     photonView.RPC("SetImageColor", RpcTarget.All);



    // }
    // RPC를 통해 이미지의 색상을 설정하는 메서드
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
                // 클릭 후 이미지 색상
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

    // RPC를 통해 이미지의 색상을 설정하는 메서드
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

    // RPC를 통해 이미지의 색상을 설정하는 메서드
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

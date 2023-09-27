using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using UnityEngine.SocialPlatforms.Impl;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameManager instance
    {
        get
        {
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<GameManager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }

    public static GameManager m_instance; // �̱����� �Ҵ�� static ����

    public GameObject playerPrefab; // ������ �÷��̾� ĳ���� ������
    public GameObject survivorRolePanel;    // ������ ������ ��� ų UI
    public GameObject traitorRolePanel;     // ����� ������ ��� ų UI
    public ProgressManager progressManager; // ���൵�� ���� ��ũ��Ʈ �¿��� ���� ��ũ��Ʈ

    public TMP_Text timerText;      // ������ ���ѽð� �ؽ�Ʈ
    public float limitTime = 900.0f; // 15���� 900��
    private float currentTime;

    public int traitorNum;      // ����� ������ ActorNumber
    public GameObject[] playerObjects;  // �÷��̾� ������Ʈ�� ����
    public List<int> escapePlayerList;  // Ż���ϴ� ������� ���ͳѹ� ����Ʈ
    public int deadPlayers = 0;

    public int slotUniqueNum = default;

    public bool isLimitOver = false;
    public bool isRepairPowerStation = false;
    public bool isRepairHeliPad = false;
    public bool isCallHeli = false;

    // �ֱ������� �ڵ� ����Ǵ�, ����ȭ �޼���
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // �ٲ㼭 ����ϱ�
        //// ���� ������Ʈ��� ���� �κ��� �����
        //if (stream.IsWriting)
        //{
        //    // ��Ʈ��ũ�� ���� score ���� ������
        //    stream.SendNext(score);
        //}
        //else
        //{
        //    // ����Ʈ ������Ʈ��� �б� �κ��� �����         

        //    // ��Ʈ��ũ�� ���� score �� �ޱ�
        //    score = (int)stream.ReceiveNext();
        //    // ����ȭ�Ͽ� ���� ������ UI�� ǥ��
        //    UIManager.instance.UpdateScoreText(score);
        //}
    }
    void Awake()
    {
        //ĳ���� ���� �տ� ���� (��ǥ x : -200f z : 287f)        
        PhotonNetwork.Instantiate("Player", new Vector3(-200f, 1f, 287f), Quaternion.identity);
        PhotonNetwork.AutomaticallySyncScene = true;
        slotUniqueNum = PhotonNetwork.LocalPlayer.ActorNumber * 4 - 4;
    }
    // Start is called before the first frame update
    void Start()
    {       
        //���� �ο��ϱ�
        if (PhotonNetwork.IsMasterClient)
        {
            traitorNum = Random.Range(1, PhotonNetwork.PlayerList.Length + 1);
            photonView.RPC("CastTraitor", RpcTarget.AllBuffered, traitorNum);
        }      
        //Ÿ�̸� ����
        currentTime = limitTime;
        StartCoroutine(UpdateTimer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    // �÷��̾�� �ڽ��� ActorNumber�� Ȯ���Ͽ� ����� ��ȣ�� ��� ����� ���� �ǳ��� ����
    [PunRPC]
    public void CastTraitor(int traitorNum_)
    {
        traitorNum = traitorNum_;
        if (PhotonNetwork.LocalPlayer.ActorNumber == traitorNum_)
        {
            traitorRolePanel.SetActive(true);
        }
        else
        {
            survivorRolePanel.SetActive(true);
        }
    }

    private IEnumerator UpdateTimer()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime; // �ð��� ���Դϴ�.

            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);

            string timeText = string.Format("{0:00}:{1:00}", minutes, seconds);

            timerText.text = timeText; // Ÿ�̸Ӹ� UI �ؽ�Ʈ�� ����մϴ�.

            yield return null; // ���� �����ӱ��� ���
        }
        isLimitOver = true;
    }

    public void RepairPowerStation()
    {
        photonView.RPC("RPCRepairPowerStation", RpcTarget.All);
    }

    [PunRPC]
    public void RPCRepairPowerStation()
    {
        isRepairPowerStation = true;
        UIManager.instance.UpdatePowerStaionText();
        progressManager.OpenRepairHeliPad();
    }

    public void RepairHelipad()
    {
        photonView.RPC("RPCRepairHelipad", RpcTarget.All);
    }

    [PunRPC]
    public void RPCRepairHelipad()
    {
        isRepairHeliPad = true;
        UIManager.instance.UpdateHeliPadText();
        progressManager.OpenCallHeliRadio();
    }

    public void CallHeli()
    {
        isCallHeli = true;
        UIManager.instance.UpdateEscapeText();
        progressManager.OpenHelicopter();
        playerObjects = GameObject.FindGameObjectsWithTag("Player");
    }

    public void DeadPlayerCheck()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber != traitorNum)
        {
            photonView.RPC("PlusDeadPlayer", RpcTarget.MasterClient);
        }

    }

    [PunRPC]
    public void PlusDeadPlayer()
    {
        deadPlayers += 1;

        if(deadPlayers >= PhotonNetwork.PlayerList.Length - 1)
        {
            // ���� ������ ���
            PhotonNetwork.LoadLevel("BadEnding");
        }
    }

    [PunRPC]
    public void LoadGoodEnding()
    {
        //Ż�� ������ ��� 
        PhotonNetwork.LoadLevel("GoodEnding");
    }
}

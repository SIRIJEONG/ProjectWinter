using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using UnityEngine.SocialPlatforms.Impl;

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

    public TMP_Text timerText;      // ������ ���ѽð� �ؽ�Ʈ
    public float limitTime = 900.0f; // 15���� 900��
    private float currentTime;

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

    // Start is called before the first frame update
    void Start()
    {
        //ĳ���� ���� �տ� ����

        //Ÿ�̸� ����
        currentTime = limitTime;
        StartCoroutine(UpdateTimer());
    }

    // Update is called once per frame
    void Update()
    {

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

        timerText.text = "�ð� ����"; // �ð��� �� �������� ���� ó��
        isLimitOver = true;
    }

    public void RepairPowerStation()
    {
        isRepairPowerStation = true;
        GFunc.GetRootObject("UiCanvas").FindChildComponent<TMP_Text>("PowerStationText").text
            = string.Format("������ ���� : ���� �Ϸ�");
    }

    public void RepairHelipad()
    {
        isRepairHeliPad = true;
        GFunc.GetRootObject("UiCanvas").FindChildComponent<TMP_Text>("HeliPadText").text
            = string.Format("�︮�е� ���� : ���� �Ϸ�");
    }

    public void CallHeli()
    {
        isCallHeli = true;
        GFunc.GetRootObject("UiCanvas").FindChildComponent<TMP_Text>("EscapeText").text
            = string.Format("Ż�� ���� ���� ��.");
    }
}

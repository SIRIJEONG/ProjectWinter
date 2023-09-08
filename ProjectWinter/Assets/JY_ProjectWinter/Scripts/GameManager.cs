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
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    public static GameManager m_instance; // 싱글톤이 할당될 static 변수

    public GameObject playerPrefab; // 생성할 플레이어 캐릭터 프리팹

    public TMP_Text timerText;      // 눈보라 제한시간 텍스트
    public float limitTime = 900.0f; // 15분은 900초
    private float currentTime;

    public bool isLimitOver = false;
    public bool isRepairPowerStation = false;
    public bool isRepairHeliPad = false;
    public bool isCallHeli = false;

    // 주기적으로 자동 실행되는, 동기화 메서드
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 바꿔서 사용하기
        //// 로컬 오브젝트라면 쓰기 부분이 실행됨
        //if (stream.IsWriting)
        //{
        //    // 네트워크를 통해 score 값을 보내기
        //    stream.SendNext(score);
        //}
        //else
        //{
        //    // 리모트 오브젝트라면 읽기 부분이 실행됨         

        //    // 네트워크를 통해 score 값 받기
        //    score = (int)stream.ReceiveNext();
        //    // 동기화하여 받은 점수를 UI로 표시
        //    UIManager.instance.UpdateScoreText(score);
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        //캐릭터 산장 앞에 생성

        //타이머 시작
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
            currentTime -= Time.deltaTime; // 시간을 줄입니다.

            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);

            string timeText = string.Format("{0:00}:{1:00}", minutes, seconds);

            timerText.text = timeText; // 타이머를 UI 텍스트로 출력합니다.

            yield return null; // 다음 프레임까지 대기
        }

        timerText.text = "시간 종료"; // 시간이 다 떨어졌을 때의 처리
        isLimitOver = true;
    }

    public void RepairPowerStation()
    {
        isRepairPowerStation = true;
        GFunc.GetRootObject("UiCanvas").FindChildComponent<TMP_Text>("PowerStationText").text
            = string.Format("발전소 상태 : 수리 완료");
    }

    public void RepairHelipad()
    {
        isRepairHeliPad = true;
        GFunc.GetRootObject("UiCanvas").FindChildComponent<TMP_Text>("HeliPadText").text
            = string.Format("헬리패드 상태 : 수리 완료");
    }

    public void CallHeli()
    {
        isCallHeli = true;
        GFunc.GetRootObject("UiCanvas").FindChildComponent<TMP_Text>("EscapeText").text
            = string.Format("탈출 차량 접근 중.");
    }
}

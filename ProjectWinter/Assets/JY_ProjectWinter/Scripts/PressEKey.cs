using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;

public class PressEKey : MonoBehaviourPun
{
    public GameObject completeUi;   // �Ϸ�! UI
    public GameObject noticeUi;     // �˸� UI
    public Text noticeText;         // �˸� UI�� �ؽ�Ʈ
    public Slider progressBar;      // E��ư ���൵
    public bool isComplete = false; // ������ �۵��� �Ϸ��ߴ���

    private bool isEPressed = false;// ������ �ִ��� Ȯ��
    private float ePressStartTime = 0f;
    private float ePressDuration = 1.5f; // 1.5�� ���� ������ ��

    private float currentValue = 0f;     // ���� FilledŸ���� ä���� ��
    private Coroutine fillingCoroutine;  
    

    private void Start()
    {
        noticeText.text = string.Format("{0}",UIManager.instance.FormatNoticeText(transform.parent));
        completeUi.SetActive(false);
        progressBar.value = 0;
        isComplete = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isComplete)
        {
            isEPressed = true;
            ePressStartTime = Time.deltaTime;

            if (fillingCoroutine != null)
            {
                StopCoroutine(fillingCoroutine);
            }

            fillingCoroutine = StartCoroutine(FillProgressBar());
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            isEPressed = false;
            if(progressBar.value < 100f)
            {
                progressBar.value = 0f;
            }
            StopCoroutine(fillingCoroutine);
        }
    }

    IEnumerator FillProgressBar()
    {
        float elapsedTime = 0f;

        while (elapsedTime < ePressDuration)
        {
            currentValue = Mathf.Lerp(0, 100, elapsedTime / ePressDuration);
            progressBar.value = currentValue;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (isEPressed)
        {
            currentValue = 100f;
            progressBar.value = currentValue;
            photonView.RPC("CallCompleteUi",RpcTarget.All);
        }
        else
        {
            // E Ű�� ������ �ʰ� 1.5�ʰ� ������ �ʱ�ȭ
            currentValue = 0f;
            progressBar.value = currentValue;
        }
    }

    [PunRPC]
    private void CallCompleteUi()
    {
        Debug.Log("E Ű�� 1.5�� ���� �������ϴ�!");
        isComplete = true;
        noticeUi.SetActive(false);
        completeUi.SetActive(true);
        Invoke("OffCompleteUi", 2.0f);
    }

    private void OffCompleteUi()
    {
        noticeUi.SetActive(true);
        progressBar.value = 0f;
        isComplete = false;
        completeUi.SetActive(false);
    }
}

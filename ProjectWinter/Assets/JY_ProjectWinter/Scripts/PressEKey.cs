using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PressEKey : MonoBehaviour
{
    public GameObject completeUi;
    public GameObject noticeUi;
    public Text noticeText;
    public Slider progressBar;
    public bool isComplete = false;

    private bool isEPressed = false;
    private float ePressStartTime = 0f;
    private float ePressDuration = 1.5f; // 1.5�� ���� ������ ��

    private float currentValue = 0f;
    private Coroutine fillingCoroutine;
    

    private void Start()
    {
        Debug.Log(transform.parent.name);
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
            CallCompleteUi();
        }
        else
        {
            // E Ű�� ������ �ʰ� 1.5�ʰ� ������ �ʱ�ȭ
            currentValue = 0f;
            progressBar.value = currentValue;
        }
    }

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

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // 싱글톤 접근용 프로퍼티
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }

    private static UIManager m_instance; // 싱글톤이 할당될 변수
    public TMP_Text powerStationText;
    public TMP_Text heliPadText;
    public TMP_Text escapeText;

    public void UpdatePowerStaionText()
    {
        powerStationText.text
            = string.Format("발전소 상태 : 수리 완료");
    }

    public void UpdateHeliPadText()
    {
        heliPadText.text = string.Format("헬리패드 상태 : 수리 완료");
    }

    public void UpdateEscapeText()
    {
        escapeText.text = string.Format("탈출 차량 접근 중.");
    }
}

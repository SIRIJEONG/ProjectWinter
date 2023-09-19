using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // �̱��� ���ٿ� ������Ƽ
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

    private static UIManager m_instance; // �̱����� �Ҵ�� ����
    public TMP_Text powerStationText;
    public TMP_Text heliPadText;
    public TMP_Text escapeText;

    public void UpdatePowerStaionText()
    {
        powerStationText.text
            = string.Format("������ ���� : ���� �Ϸ�");
    }

    public void UpdateHeliPadText()
    {
        heliPadText.text = string.Format("�︮�е� ���� : ���� �Ϸ�");
    }

    public void UpdateEscapeText()
    {
        escapeText.text = string.Format("Ż�� ���� ���� ��.");
    }

    public string FormatNoticeText(Transform transform_)
    {
        if(transform_.CompareTag("Operate"))
        {
            //text_.text = string.Format("�۵��ϱ�");
            return "�۵��ϱ�";
        }
        else if(transform_.CompareTag("Repair"))
        {
            //text_.text = string.Format("�����ϱ�");
            return "�����ϱ�";
        }
        return "�˸� ����";
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public enum PositionState
    {
        North,
        South,
        East,
        West
    }

    public PositionState positionState;

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

    public Image[] miniMapImg;  // �̴ϸ� �̹��� �迭 (���ʴ�� ��,��,��,��)
    private Color standingColor = new Color(0.443f, 0.443f, 0.443f);
    private Color basicColor = new Color (1f, 1f, 1f);
    public Text powerStationText;
    public Text heliPadText;
    public Text escapeText;

    private void Start()
    {
        positionState = PositionState.North;
    }

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
        else if(transform_.CompareTag("Box"))
        {
            return "�����";
        }
        return "�˸� ����";
    }

    public void ChangeMiniMap()
    {
        for (int i = 0; i < miniMapImg.Length; i++)
        {
            miniMapImg[i].color = basicColor;
        }
        switch (positionState)
        {
            case PositionState.North:
                miniMapImg[0].color = standingColor;
                break;
            case PositionState.South:
                miniMapImg[1].color = standingColor;
                break;
            case PositionState.East:
                miniMapImg[2].color = standingColor;
                break;
            case PositionState.West:
                miniMapImg[3].color = standingColor;
                break;
            default:
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SG_PowerStationItemImageColorSet : MonoBehaviour
{
    private GameObject topParentObj;
    private Image itemCountImage;
    private TextMeshProUGUI itemCountText;

    Color offColor;
    Color halfColor;
    Color onColor;

    // Start is called before the first frame update
    void Start()
    {
        GetThisTopParentObj();  // �ֻ��� ������Ʈ�� ������
        SelfDiagnosis();        // �÷��̾ ����â��� �̱���� �ʿ���⿡ ������ ��ũ��Ʈ ����
        ColorSet();             // ������ ������ �÷��� �� ��������
        ColorOff();             // �︮���� ������� �������� ī��Ʈ �� ī��Ʈ �̹����� �ʿ���⶧���� ���� Off ����
        
    }

    private void GetThisTopParentObj()  //�ֻ��� �θ� ������Ʈ �±׸� �������� ���� ã�� ����
    {
        topParentObj = this.gameObject;
        
        while (topParentObj.transform.parent != null)   // �θ������Ʈ�� ������ ���� ��
        {
            topParentObj = topParentObj.transform.parent.gameObject;
        }
    }

    private void SelfDiagnosis() // �ڽ��� �����ҳ� �︮��尡 �ƴ϶�� ����� �ʿ䰡 ���⿡ ��ũ��Ʈ ����
    {

        if (!topParentObj.CompareTag("PowerStation") && !topParentObj.CompareTag("HeliPad"))
        {
            enabled = false;
        }
        else { /*PASS*/ }
    }


    private void ColorSet()
    {
        onColor = new Color(1f, 1f, 1f, 1f);
        halfColor = new Color(1f, 1f, 1f, 0.5f);
        offColor = new Color(1f, 1f, 1f, 0f);
    }


    private void ColorOff()
    {
        // �����ҿ� �︮���� �Ʒ��� �ٸ� Text�� ī��Ʈ ���ֱ� ������ A �� �������� �Ⱥ��̵�����
        // �����ϴٰ� Null���츦 ����� ������ ����
        if (topParentObj.CompareTag("PowerStation") || topParentObj.CompareTag("HeliPad"))
        {
            itemCountImage = transform.GetChild(0).GetComponent<Image>();
            itemCountText = itemCountImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            itemCountImage.color = offColor;
            itemCountText.color = offColor;
        }
        else { /*PASS*/ }
    }
}

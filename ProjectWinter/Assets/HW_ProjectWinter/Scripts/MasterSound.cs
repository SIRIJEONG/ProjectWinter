using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterSound : MonoBehaviour
{
    public Slider masterVolumeSlider; // ������ ������ �����ϴ� �����̴�
    private float initialMasterVolume; // �ʱ� ������ ���� ����

    void Start()
    {
        // �ʱ� ������ ������ ����
        initialMasterVolume = AudioListener.volume;

        // �����̴��� ���� �ʱ� ������ �������� ����
        masterVolumeSlider.value = initialMasterVolume;

        // �����̴��� ���� ����� ������ �̺�Ʈ �ڵ鷯 ȣ��
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeSliderChanged);
    }

    // �����̴� ���� ����� �� ȣ��Ǵ� �޼���
    void OnMasterVolumeSliderChanged(float value)
    {
        // �����̴� ���� AudioListener.volume�� �����Ͽ� ������ ���� ����
        AudioListener.volume = value;
    }
}

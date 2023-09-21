using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class BackGroundSound : MonoBehaviour
{
    public VideoPlayer video;
    public Slider volumeSlider; // �����̴� ������Ʈ

    private void Start()
    {
        // �����̴��� ���� ����� �� �̺�Ʈ �ڵ鷯 ���
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
    }

    // �����̴� ���� ����� �� ȣ��Ǵ� �޼���
    void OnVolumeSliderChanged(float value)
    {
        // �����̴� ������ ���� �÷��̾��� ���� ����
        video.SetDirectAudioVolume(0, value);
    }
}

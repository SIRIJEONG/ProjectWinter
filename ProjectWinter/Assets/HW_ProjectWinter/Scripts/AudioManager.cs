using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip loopedAudioClip; // 반복 재생할 오디오 클립

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = loopedAudioClip;
        audioSource.loop = true; // 반복 재생 활성화
        audioSource.Play(); // 오디오 재생 시작
    }
}

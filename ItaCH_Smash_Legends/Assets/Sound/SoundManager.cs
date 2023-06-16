using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;
    private AudioSource[] _audioSources = new AudioSource[(int)SoundType.NumOfSoundType];
    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    private StringBuilder _stringBuilder;

    //테스트 코드. 추후 위치 변경 예정
    private void Awake()
    {
        InitSoundManagerSettings();
    }
    public void InitSoundManagerSettings()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        for (int i = 0; i < (int)SoundType.NumOfSoundType; i++)
        {
            GameObject audioSource = new GameObject { };
            audioSource.name = Enum.GetName(typeof(SoundType), i);
            _audioSources[i] = audioSource.AddComponent<AudioSource>();
            audioSource.transform.parent = transform;
        }

        _audioSources[(int)SoundType.BGM].loop = true;
        _stringBuilder = new StringBuilder();

    }
    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        _audioClips.Clear();
    }

    public void Play(AudioClip audioClip, SoundType soundType = SoundType.SFX)
    {
        AudioSource audioSource = _audioSources[(int)soundType];
        audioSource.clip = audioClip;

        if (soundType == SoundType.BGM)
        {
            audioSource.Stop();
            audioSource.Play();
        }
        else
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void Play(string name, SoundType soundType = SoundType.SFX)
    {
        _stringBuilder.Clear();
        _stringBuilder.Append("Sound/");
        if (soundType == SoundType.BGM)
        {
            _stringBuilder.Append("BGM/");
        }
        else
        {
            _stringBuilder.Append("SFX/");
        }
        _stringBuilder.Append(name);
        AudioClip audioClip = Resources.Load<AudioClip>(_stringBuilder.ToString());
        Play(audioClip, soundType);
    }
}
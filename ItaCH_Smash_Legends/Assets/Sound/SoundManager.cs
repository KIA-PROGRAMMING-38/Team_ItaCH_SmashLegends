using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;
    private AudioSource[] _audioSources = new AudioSource[(int)SoundType.NumOfSoundType];
    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    private StringBuilder _stringBuilder;
    private AudioMixer _audioMixer;

    //�׽�Ʈ �ڵ�. ���� ��ġ ���� ����
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

        _audioMixer = Resources.Load<AudioMixer>("Sound/AudioMixer");
        _stringBuilder = new StringBuilder();

        for (int i = 0; i < (int)SoundType.NumOfSoundType; i++)
        {
            GameObject audioSource = new GameObject { };
            string soundType = Enum.GetName(typeof(SoundType), i);
            audioSource.name = soundType;
            _audioSources[i] = audioSource.AddComponent<AudioSource>();
            audioSource.transform.parent = transform;
            _audioSources[i].outputAudioMixerGroup = _audioMixer.FindMatchingGroups(soundType)[0];
        }

        _audioSources[(int)SoundType.BGM].loop = true;
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
        _stringBuilder.Append(soundType);
        _stringBuilder.Append("/");
        _stringBuilder.Append(name);
        AudioClip audioClip = Resources.Load<AudioClip>(_stringBuilder.ToString());
        Play(audioClip, soundType);
    }

    public void SetVolume(SoundType soundType, float value)
    {
        //���� ���� ����ϴ� ���� ���� ��. ������ ����� 50�� �ƴ� 20�� ����ϳ�, ��ȭ�� �ѷ����� �ʾ� 50�� �����.
        _audioMixer.SetFloat(soundType.ToString(), Mathf.Log10(value) * 50);
    }
}
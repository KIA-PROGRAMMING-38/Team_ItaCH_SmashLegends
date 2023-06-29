using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;
    private AudioSource[] _audioSources = new AudioSource[(int)SoundType.NumOfSoundType - 1];
    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    private StringBuilder _stringBuilder;
    private AudioMixer _audioMixer;

    #region ���� ���
    private const string _None3DSoundRootFolderPath = "Sound/";
    private const string _3DSoundRootFolderPath = "Sound/CHAR/";
    #endregion

    //�׽�Ʈ �ڵ�. ���� ��ġ ���� ����
    private void Awake()
    {
        InitSoundManagerSettings();
    }
    public void InitSoundManagerSettings()
    {
        #region �̱���
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        #endregion
        _audioMixer = Resources.Load<AudioMixer>("Sound/AudioMixer");
        _stringBuilder = new StringBuilder();

        for (int i = 0; i < (int)SoundType.NumOfSoundType - 1; i++)
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
    public void Clear(AudioSource[] audioSources, Dictionary<string, AudioClip> audioClips)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        audioClips.Clear();
    }

    //3D������ �ʿ���� �Ҹ� ��� (�����Ŭ������ ���)
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

    //3D������ �ʿ���� �Ҹ� ��� (�̸����� ���)
    public void Play(string name, SoundType soundType = SoundType.SFX)
    {
        if (soundType.Equals(SoundType.SFX) && _audioClips.ContainsKey(name))
        {
            Play(_audioClips[name], soundType);
            return;
        }
        SetSoundPath(_None3DSoundRootFolderPath, name, soundType);
        AudioClip audioClip = Resources.Load<AudioClip>(_stringBuilder.ToString());
        if (soundType.Equals(SoundType.SFX))
        {
            _audioClips.Add(name, audioClip);
        }
        Play(audioClip, soundType);
    }

    //3D���� ����� �ʿ��� �Ҹ����. ĳ���Ͱ� ��ü������ ������ �ִ� �Ҹ� Dictionary�� �˻��� �� Play ����.
    //name�� Peter/Die00 �̷������� ĳ�����̸�/��Ȳ���� ����.
    public void Play(string name, Dictionary<string, AudioClip> dictionary, AudioSource audioSource, SoundType soundType)
    {
        if (dictionary.ContainsKey(name))
        {
            audioSource.PlayOneShot(dictionary[name]);
        }
        else
        {
            SetSoundPath(_3DSoundRootFolderPath, name, soundType);
            AudioClip audioClip = Resources.Load<AudioClip>(_stringBuilder.ToString());
            if(audioClip.Equals(null))
            {
                return;
            }
            dictionary[name] = audioClip;
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void SetVolume(SoundType soundType, float value)
    {
        //���� ���� ����ϴ� ���� ���� ��. ������ ����� 50�� �ƴ� 20�� ����ϳ�, ��ȭ�� �ѷ����� �ʾ� 50�� �����.
        _audioMixer.SetFloat(soundType.ToString(), Mathf.Log10(value) * 50);
    }

    private void SetSoundPath(string rootFolderPath, string name, SoundType soundType)
    {
        _stringBuilder.Clear();
        _stringBuilder.Append(rootFolderPath);
        _stringBuilder.Append(soundType);
        _stringBuilder.Append("/");
        _stringBuilder.Append(name);
    }
}
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

    #region 폴더 경로
    private const string _None3DSoundRootFolderPath = "Sound/";
    private const string _3DSoundRootFolderPath = "Sound/CHAR/";
    #endregion

    //테스트 코드. 추후 위치 변경 예정
    private void Awake()
    {
        InitSoundManagerSettings();
    }
    public void InitSoundManagerSettings()
    {
        #region 싱글톤
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

    //3D음향이 필요없는 소리 재생 (오디오클립으로 재생)
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

    //3D음향이 필요없는 소리 재생 (이름으로 재생)
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

    //3D음향 재생이 필요한 소리재생. 캐릭터가 자체적으로 가지고 있는 소리 Dictionary를 검사한 후 Play 실행.
    //name은 Peter/Die00 이런식으로 캐릭터이름/상황으로 설정.
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
        //가장 많이 사용하는 볼륨 변경 식. 원래는 기울기로 50이 아닌 20을 사용하나, 변화가 뚜렷하지 않아 50을 사용함.
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
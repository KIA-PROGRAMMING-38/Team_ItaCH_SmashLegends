using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;
using Util.Enum;

public class SoundSource : MonoBehaviour
{
    private Dictionary<string, AudioClip>[] _audioClips;
    private AudioSource[] _audioSources;
    private AudioMixer _audioMixer;
    private CharacterType _characterType;
    private StringBuilder _stringBuilder;

    private float time;

    private void Awake()
    {
        _audioMixer = Resources.Load<AudioMixer>("Sound/AudioMixer");
        _stringBuilder = new StringBuilder();
        InitSoundSourceSettings(CharacterType.Alice);
    }
    private void Update()
    {
        if (time > 1)
        {
            Play("Jump", SoundType.SFX);
            time = 0;
        }
        time += Time.deltaTime;
    }
    public void InitSoundSourceSettings(CharacterType characterType)
    {
        _audioClips = new Dictionary<string, AudioClip>[(int)SoundType.NumOfSoundType];
        _audioSources = new AudioSource[(int)SoundType.NumOfSoundType];
        _characterType = characterType;

        for (int i = (int)SoundType.SFX; i < (int)SoundType.NumOfSoundType; ++i)
        {
            GameObject audioSource = new GameObject { };
            string soundType = Enum.GetName(typeof(SoundType), i);
            audioSource.name = soundType;
            _audioClips[i] = new Dictionary<string, AudioClip>();
            _audioSources[i] = audioSource.AddComponent<AudioSource>();
            audioSource.transform.parent = transform;
            _audioSources[i].outputAudioMixerGroup = _audioMixer.FindMatchingGroups(soundType)[0];
        }
    }

    public void Play(string name, SoundType soundType)
    {
        _stringBuilder.Clear();
        _stringBuilder.Append(_characterType);
        _stringBuilder.Append("/");
        _stringBuilder.Append(name);
        SoundManager._instance.Play(_stringBuilder.ToString(), _audioClips[(int)soundType], _audioSources[(int)soundType], soundType);
    }

    public void Play(string name, int randomInt, SoundType soundType = SoundType.Voice)
    {
        _stringBuilder.Clear();
        _stringBuilder.Append(_characterType);
        _stringBuilder.Append("/");
        _stringBuilder.Append(name);
        _stringBuilder.Append(String.Format($"{randomInt:00}"));
        SoundManager._instance.Play(_stringBuilder.ToString(), _audioClips[(int)soundType], _audioSources[(int)soundType], soundType);
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private AudioSource[] _audioSources = new AudioSource[(int)SoundType.NumOfSoundType];
    private AudioMixer _audioMixer;

    private string[] _defaultAttacks = { "DefaultAttack00", "DefaultAttack01", "DefaultAttack02" };
    private string[] _dies = { "Die00", "Die01", "Die02", "Die03" };
    private string[] _heavyAttacks = { "HeavyAttack00", "HeavyAttack01", "HeavyAttack02", "HeavyAttack03" };
    private string[] _hits = { "Hit00", "Hit01", "Hit02", "Hit03", "Hit04" };
    private string[] _hitUps = { "HitUp00", "HitUp01", "HitUp02", "HitUp03", "HitUp04" };
    private string[] _jumps = { "Jump00", "Jump01" };
    private string[] _jumpAttacks = { "JumpAttack00", "JumpAttack01", "JumpAttack02" };
    private string[] _lastAttacks = { "LastAttack00", "LastAttack01", "LastAttack02", };
    private string[] _lobbys = { "Lobby00", "Lobby01", "Lobby02" };
    private string[] _revives = { "Revive00", "Revive01" };
    private string[] _selecteds = { "Selected00", "Selected01", "Selected02", "Selected03" };
    private string[] _skiiAttacks = { "SkillAttack00", "SkillAttack01", "SkillAttack02", "SkillAttack03" };

    private Dictionary<VoiceType, string[]> _legendVoices = new Dictionary<VoiceType, string[]>();

    public void Init()
    {
        SetLegendVoice();

        for (int index = 0; index < _audioSources.Length; ++index)
        {
            GameObject gameObject = new GameObject();
            gameObject.name = Enum.GetName(typeof(SoundType), index);
            gameObject.transform.parent = transform;
            _audioSources[index] = gameObject.AddComponent<AudioSource>();
        }
        _audioSources[(int)SoundType.BGM].loop = true;
    }

    public void Clear(AudioSource[] audioSources)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
    }

    public void Play(SoundType sound, string fileName = null, LegendType legend = LegendType.None,
        VoiceType voice = VoiceType.MaxCount)
    {
        if (sound == SoundType.Voice)
        {
            AudioClip audioClip = GetVoiceAudioClip(legend, voice);
            _audioSources[(int)sound].PlayOneShot(audioClip);
            return;
        }
        else
        {
            AudioClip audioClip = Managers.ResourceManager.GetAudioClip(fileName, sound, legend);

            if (sound == SoundType.BGM)
            {
                _audioSources[(int)sound].Stop();
                _audioSources[(int)sound].clip = audioClip;
                _audioSources[(int)sound].Play();

                return;
            }

            else
            {
                _audioSources[(int)sound].PlayOneShot(audioClip);
                return;
            }
        }
    }

    private AudioClip GetVoiceAudioClip(LegendType legend, VoiceType voice)
    {
        AudioClip audioClip;

        if (voice == VoiceType.Win)
        {
            audioClip = Managers.ResourceManager.GetAudioClip(StringLiteral.VOICE_WIN, SoundType.Voice, legend);
        }
        else
        {
            int index = UnityEngine.Random.Range(0, _legendVoices[voice].Length);
            string result = _legendVoices[voice][index];

            audioClip = Managers.ResourceManager.GetAudioClip(result, SoundType.Voice, legend);
        }

        return audioClip;
    }

    public void SetVolume(SoundType soundType, float value)
    {
        //가장 많이 사용하는 볼륨 변경 식. 원래는 기울기로 50이 아닌 20을 사용하나, 변화가 뚜렷하지 않아 50을 사용함.
        _audioMixer.SetFloat(soundType.ToString(), Mathf.Log10(value) * 50);
    }

    private void SetLegendVoice()
    {
        _legendVoices[VoiceType.DefaultAttack] = _defaultAttacks;
        _legendVoices[VoiceType.Die] = _dies;
        _legendVoices[VoiceType.HeavyAttack] = _heavyAttacks;
        _legendVoices[VoiceType.Hit] = _hits;
        _legendVoices[VoiceType.HitUp] = _hitUps;
        _legendVoices[VoiceType.Jump] = _jumps;
        _legendVoices[VoiceType.JumpAttack] = _jumpAttacks;
        _legendVoices[VoiceType.LastAttack] = _lastAttacks;
        _legendVoices[VoiceType.Lobby] = _lobbys;
        _legendVoices[VoiceType.Revive] = _revives;
        _legendVoices[VoiceType.Selected] = _selecteds;
        _legendVoices[VoiceType.SkillAttack] = _skiiAttacks;
    }
}
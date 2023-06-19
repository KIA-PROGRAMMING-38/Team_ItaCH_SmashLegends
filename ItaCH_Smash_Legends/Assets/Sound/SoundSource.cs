using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource
{
    private Dictionary<string, AudioClip> _audioClips;
    private AudioSource _audioSource;

    public Dictionary<string, AudioClip> audioClip { get => _audioClips; set { _audioClips = value; } }
    public AudioSource AudioSource { get => _audioSource ; set { _audioSource = value; } }
}

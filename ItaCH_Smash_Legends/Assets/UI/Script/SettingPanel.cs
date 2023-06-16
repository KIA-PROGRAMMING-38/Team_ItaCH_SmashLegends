using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private VolumeSlider[] _volumeSliders;

    //�׽�Ʈ �ڵ�. ���� ���� �ܺο��� �޾ƿ� ����.
    void Awake()
    {
        InitSettingPanel(1);
    }

    public void InitSettingPanel(float defaultVolume)
    {
        for(int i = 0; i < (int)SoundType.NumOfSoundType; ++i)
        {
            _volumeSliders[i].InitVolumeSliderSetting((SoundType)i, defaultVolume);
        }
    }
}

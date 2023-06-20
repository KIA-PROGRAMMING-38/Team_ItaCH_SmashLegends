using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SettingPanel : MonoBehaviour, IPanel
{
    [SerializeField] private VolumeSlider[] _volumeSliders;

    public void InitPanelSettings(LobbyUI lobbyUI)
    {
        for(int i = 0; i < (int)SoundType.NumOfSoundType; ++i)
        {
            _volumeSliders[i].InitVolumeSliderSetting((SoundType)i, lobbyUI.DefaultVolume);
        }
    }
}

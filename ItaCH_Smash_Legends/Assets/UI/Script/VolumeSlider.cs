using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Text _volumeAmount;

    public void InitVolumeSliderSettings()
    {
        _volumeAmount = transform.Find("Text").GetComponent<Text>();
    }

    public void ChangeText(float value)
    {
        int percentageValue = (int)(value * 100);
        _volumeAmount.text = {value};
    }
}

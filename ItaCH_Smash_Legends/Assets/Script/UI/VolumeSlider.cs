using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private TextMeshProUGUI _volumeAmount;
    private Slider _slider;
    private SoundType _soundType;

    public void InitVolumeSliderSetting(SoundType soundType, float defaultVolume)
    {
        _volumeAmount = transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        _slider = transform.GetChild(0).GetComponent<Slider>();
        _slider.value = defaultVolume;
        _slider.minValue = 0.0001f;
        _slider.onValueChanged.RemoveAllListeners();
        _slider.onValueChanged.AddListener(ChangeVolume);
        ChangeText();
        _soundType = soundType;
    }
    public void ChangeText()
    {
        int percentageValue = (int)(_slider.value * 100);
        _volumeAmount.text = $"{percentageValue}";
    }

    public void ChangeVolume(float value)
    {
        Managers.SoundManager.SetVolume(_soundType, value);
    }
}

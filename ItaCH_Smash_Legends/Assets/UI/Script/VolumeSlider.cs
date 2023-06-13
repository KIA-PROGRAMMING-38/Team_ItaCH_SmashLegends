using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private TextMeshProUGUI _volumeAmount;
    private Slider _slider;

    //�׽�Ʈ �ڵ�. ���� �ʿ��� �κп��� ȣ���� ����
    private void Start()
    {
        InitVolumeSliderSetting();
    }
    public void InitVolumeSliderSetting()
    {
        _volumeAmount = transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        _slider = transform.GetChild(0).GetComponent<Slider>();
        ChangeText();
    }
    public void ChangeText()
    {
        int percentageValue = (int)(_slider.value * 100);
        _volumeAmount.text = $"{percentageValue}";
    }
}

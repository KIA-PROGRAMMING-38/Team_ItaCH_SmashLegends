using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LegendSelectUI : MonoBehaviour
{
    private int _legendIndex;
    private Image _portrait;
    private Image _frame;
    private Image _selectedFrame;
    private Button _button;

    //추후, 레전드의 인덱스 번호로 선택한 레전드를 확인할 수 있음.
    public event Action<int> OnSelectLegend;

    public void InitLegendSelectUI(int currentIndex, Sprite portraitSprite)
    {
        _legendIndex = currentIndex;
        _portrait = transform.GetChild(0).GetComponent<Image>();
        _portrait.sprite = portraitSprite;
        _frame = GetComponent<Image>();
        _selectedFrame = transform.GetChild(2).GetComponent<Image>();
        DisableSelectFrame();
        _button = GetComponent<Button>();
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(OnPressButton);
    }

    public void OnPressButton()
    {
        OnSelectLegend?.Invoke(_legendIndex);
    }

    public void EnableSelectFrame() => _selectedFrame.enabled = true;
    public void DisableSelectFrame() => _selectedFrame.enabled = false;

    public void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}

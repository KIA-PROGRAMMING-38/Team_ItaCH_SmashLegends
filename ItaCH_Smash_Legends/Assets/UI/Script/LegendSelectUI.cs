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

    //����, �������� �ε��� ��ȣ�� ������ �����带 Ȯ���� �� ����.
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

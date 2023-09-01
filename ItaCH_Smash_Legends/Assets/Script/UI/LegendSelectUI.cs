using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LegendSelectUI : MonoBehaviour
{
    private int _legendIndex;
    private Image _portrait;
    private Image _frame;
    private Image _selectedFrame;
    private Button _button;
    private TextMeshProUGUI _legendName;
    private int _selectedLegend;

    public event Action<int> OnSelectLegend;

    public void InitLegendSelectUI(int currentIndex, Sprite portraitSprite, string legendName)
    {
        _selectedLegend = (int)Managers.LobbyManager.UserLocalData.SelectedLegend;

        _legendIndex = currentIndex;
        _portrait = transform.GetChild(0).GetComponent<Image>();
        _portrait.sprite = portraitSprite;
        _frame = GetComponent<Image>();
        _selectedFrame = transform.GetChild(2).GetComponent<Image>();
        DisableSelectFrame();
        _button = GetComponent<Button>();
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(OnPressButton);
        _button.onClick.AddListener(() => Managers.SoundManager.Play(SoundType.SFX, StringLiteral.SFX_BUTTON));
        _legendName = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        _legendName.text = legendName;
        if (currentIndex.Equals(_selectedLegend))
        {
            EnableSelectFrame();
        }
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

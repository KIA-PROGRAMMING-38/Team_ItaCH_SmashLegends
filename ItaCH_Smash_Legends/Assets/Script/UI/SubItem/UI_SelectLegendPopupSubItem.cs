using UnityEngine;

public class UI_SelectLegendPopupSubItem : UIBase
{
    enum Texts
    {
        LegendNameText
    }

    enum Images
    {
        SelectButton
    }

    enum Buttons
    {
        SelectButton
    }

    enum GameObjects
    {
        SelectedLegendFrame
    }

    private int _buttonId;

    public override void Init()
    {
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetObject((int)GameObjects.SelectedLegendFrame).SetActive(false);
        GetButton((int)Buttons.SelectButton).gameObject.BindEvent(OnSelectLegendButton);
    }

    public void SetInfo(int id)
    {
        _buttonId = id;

        GetImage((int)Images.SelectButton).sprite = Managers.ResourceManager.GetLegendSprite(StringLiteral.UI_SELECT_LEGEND_POPUP, (LegendType)id);
        GetText((int)Texts.LegendNameText).text = Managers.DataManager.LegendStats[id].LegendNameKOR;

        RefreshUI();
    }

    public void OnSelectLegendButton()
    {
        Managers.LobbyManager.UserLocalData.SelectedLegend = (LegendType)_buttonId;
        Managers.SoundManager.Play(SoundType.SFX, StringLiteral.SFX_BUTTON);

        Managers.UIManager.FindPopup<UI_SelectLegendPopup>().RefreshUI();
    }

    public void RefreshUI()
    {
        if (_buttonId == (int)Managers.LobbyManager.UserLocalData.SelectedLegend)
        {
            GetObject((int)GameObjects.SelectedLegendFrame).SetActive(true);
        }

        else
        {
            GetObject((int)GameObjects.SelectedLegendFrame).SetActive(false);
        }
    }
}
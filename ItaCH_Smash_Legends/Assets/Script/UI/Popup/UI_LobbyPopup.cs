using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LobbyPopup : UIPopup
{
    enum Texts
    {
        UserNameText
    }

    enum Buttons
    {
        SelectLegendButton,
        StartGameModeButton,
        SettingsButton
    }

    private static LegendType _currentLobbyLegend;
    private static Transform _legendSpawnPoint;

    public override void Init()
    {
        base.Init();

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetText((int)Texts.UserNameText).text = Managers.LobbyManager.UserLocalData.Name;

        GetButton((int)Buttons.SelectLegendButton).gameObject.BindEvent(() => Managers.UIManager.ShowPopupUI<UI_SelectLegendPopup>());
        GetButton((int)Buttons.StartGameModeButton).gameObject.BindEvent(ClosePopupUI);
        GetButton((int)Buttons.SettingsButton).gameObject.BindEvent(() => Managers.UIManager.ShowPopupUI<UI_SettingsPopup>());

        SetLobbyWorld();
    }

    public void SetLobbyWorld()
    {
        GameObject lobbyWorld = Managers.ResourceManager.Instantiate(StringLiteral.LOBBY_WORLD_MAP_PREFAB_PATH);
        _legendSpawnPoint = lobbyWorld.transform.GetChild(0).GetChild(0);

        SetLobbyLegendModel();
    }

    private void SetLobbyLegendModel()
    {
        LegendType userSelectedLegend = Managers.LobbyManager.UserLocalData.SelectedLegend;

        GameObject lobbyLegendModel = Managers.ResourceManager.GetLobbyLegendPrefab(userSelectedLegend);
        Managers.ResourceManager.Instantiate(lobbyLegendModel, _legendSpawnPoint);

        _currentLobbyLegend = userSelectedLegend;
    }

    public void RefreshUI()
    {
        SetLobbyLegendModel();
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
        Managers.UIManager.ShowPopupUI<UI_MatchingPopup>();
        Managers.LobbyManager.Connect();
    }
}

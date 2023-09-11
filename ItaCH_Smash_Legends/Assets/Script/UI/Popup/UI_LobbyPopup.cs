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

    private GameObject[] _legendModels = new GameObject[(int)LegendType.MaxCount];
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

        _currentLobbyLegend = Managers.LobbyManager.UserLocalData.SelectedLegend;

        for (int id = (int)LegendType.Alice; id < (int)LegendType.MaxCount; ++id)
        {
            GameObject lobbyLegendModel = Managers.ResourceManager.GetLobbyLegendPrefab((LegendType)id);
            lobbyLegendModel.SetActive(false);
            _legendModels[id] = Managers.ResourceManager.Instantiate(lobbyLegendModel, _legendSpawnPoint);

            if (id == (int)_currentLobbyLegend)
            {
                _legendModels[id].SetActive(true);
            }
        }

        SetLobbyLegendModel();
    }

    private void SetLobbyLegendModel()
    {
        LegendType userSelectedLegend = Managers.LobbyManager.UserLocalData.SelectedLegend;
        Managers.SoundManager.Play(SoundType.Voice, legend: userSelectedLegend, voice: VoiceType.Lobby);

        if (_currentLobbyLegend == userSelectedLegend)
        {
            return;
        }

        _legendModels[(int)_currentLobbyLegend].SetActive(false);
        _legendModels[(int)userSelectedLegend].SetActive(true);
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

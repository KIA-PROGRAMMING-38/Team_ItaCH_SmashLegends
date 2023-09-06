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
        BindText(typeof(Texts));        
        BindButton(typeof(Buttons));

        GetText((int)Texts.UserNameText).text = Managers.LobbyManager.UserLocalData.Name;

        GetButton((int)Buttons.SelectLegendButton).gameObject.BindEvent(() => Managers.UIManager.ShowPopupUI<UI_SelectLegendPopup>());
        GetButton((int)Buttons.StartGameModeButton).gameObject.BindEvent(() => Managers.UIManager.ShowPopupUI<UI_MatchingPopup>());
        GetButton((int)Buttons.SettingsButton).gameObject.BindEvent(() => Managers.UIManager.ShowPopupUI<UI_SettingsPopup>());

        SetLobbyWorld();
    }

    private void SetLobbyWorld()
    {        
        GameObject lobbyWorld = Managers.ResourceManager.Instantiate(StringLiteral.LOBBY_WORLD_MAP_PREFAB_PATH);
        _legendSpawnPoint = lobbyWorld.transform.GetChild(0).GetChild(0);

        SetLobbyLegendModel();
    }

    private void SetLobbyLegendModel()
    {
        LegendType userSelectedLegend = Managers.LobbyManager.UserLocalData.SelectedLegend;
        
        if(userSelectedLegend == _currentLobbyLegend)
        {
            return;
        }

        GameObject lobbyLegendModel = Managers.ResourceManager.GetLobbyLegendPrefab(userSelectedLegend);
        Managers.ResourceManager.Instantiate(lobbyLegendModel, _legendSpawnPoint);

        _currentLobbyLegend = userSelectedLegend;
    }

    public void RefreshUI()
    {
        SetLobbyLegendModel();
    }

    /* 기존 성재 로직에서 결과창 이후 모델 변경 사항 초기화 해주는 코드
     * public void ResetModelTransform()
    {
        foreach (GameObject legendModel in _legendModels)
        {
            Transform modelTransform = legendModel.transform;
            modelTransform.SetParent(_spawnPoint);
            modelTransform.localPosition = Vector3.zero;
            modelTransform.localScale = new Vector3(1, 1, 1);
            modelTransform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
    */
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UI_DuelModePopup : UIPopup
{
    enum Texts
    {
        GameTimerText,
        PlayerRespawnTimerText
    }

    enum Images
    {
        PlayerRespawnTimerFill
    }

    enum GameObjects
    {
        GameTimer,
        PlayerRespawnTimer,
        Profile
    }
    enum UIProfiles
    {
        BlueTeamProfile,
        RedTeamProfile
    }

    private const float MAX_FILL_AMOUNT = 1f;
    private const float DEFAULT_FILL_AMOUNT = 0f;

    private List<UI_Profile> _profiles = new List<UI_Profile>();

    public override void Init()
    {
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindObject(typeof(GameObjects));
        Bind<UI_Profile>(typeof(UIProfiles));

        PopulateProfile();

        RefreshPopupUI();
    }

    private void PopulateProfile()
    {
        _profiles.Clear();

        GameObject parentObject = GetObject((int)GameObjects.Profile);

        foreach (Transform child in parentObject.transform)
        {
            Managers.ResourceManager.Destroy(child.gameObject);
        }

        int maxTeamCount = Managers.StageManager.CurrentGameMode.MaxTeamCount;

        for (int teamIndex = 0; teamIndex < maxTeamCount; ++teamIndex)
        {
            UI_Profile profileItem = Managers.UIManager.MakeSubItem<UI_Profile>(parentObject.transform);
            profileItem.SetInfo(teamIndex);

            _profiles.Add(profileItem);
        }
    }

    private void RefreshPopupUI()
    {
        RefreshGameTimer();
        RefreshProfile();
    }

    public void RefreshGameTimer()
    {
        GetText((int)Texts.GameTimerText).text = $"{Managers.StageManager.RemainGameTime / 60} : {Managers.StageManager.RemainGameTime % 60}";
    }

    public void RefreshPlayerRespawnTimer()
    {
        // To Do : When Player.OwnedLegend.OnDie.Invoke();
    }

    private async void RefreshPlayerTimerFill(float respawnTime)
    {
        GetImage((int)Images.PlayerRespawnTimerFill).fillAmount = DEFAULT_FILL_AMOUNT;
        await Utils.ChangeFillAmountGradually(MAX_FILL_AMOUNT, respawnTime, GetImage((int)Images.PlayerRespawnTimerFill));
    }

    public void RefreshProfile()
    {
        foreach (UI_Profile profileItem in _profiles)
        {
            profileItem.RefreshUpdatedInGameItems();
        }
    }
}
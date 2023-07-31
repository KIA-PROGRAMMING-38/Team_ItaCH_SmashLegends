using Cysharp.Threading.Tasks;
using System.Collections.Generic;
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
    private const float DEFAULT_FILL_AMOUNT = 0.1f;

    private List<UI_Profile> _profiles = new List<UI_Profile>();

    public override void Init()
    {
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindObject(typeof(GameObjects));
        Bind<UI_Profile>(typeof(UIProfiles));

        PopulateProfile();

        GetObject((int)GameObjects.PlayerRespawnTimer).SetActive(false);
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
        GetText((int)Texts.GameTimerText).text = $"{Managers.StageManager.RemainGameTime / 60:D2}:{Managers.StageManager.RemainGameTime % 60:D2}";
    }

    public void RefreshPlayerRespawnTimer(float respawnTime)
    {
        // To Do : When Player.OwnedLegend.OnDie.Invoke();        
        GetObject((int)GameObjects.PlayerRespawnTimer).SetActive(true);

        RefreshPlayerRespawnTimerFillTask(respawnTime).Forget();
        RefreshPlayerRespawnTimerTextTask(respawnTime).Forget();
    }

    private async UniTask RefreshPlayerRespawnTimerFillTask(float respawnTime)
    {
        GetImage((int)Images.PlayerRespawnTimerFill).fillAmount = DEFAULT_FILL_AMOUNT;
        await Utils.ChangeFillAmountGradually(MAX_FILL_AMOUNT, respawnTime, GetImage((int)Images.PlayerRespawnTimerFill));
    }

    private const int ONE_SECOND = 1000;

    private async UniTask RefreshPlayerRespawnTimerTextTask(float respawnTime)
    {
        float elapsedTime = 0;
        while (elapsedTime < respawnTime)
        {
            GetText((int)Texts.PlayerRespawnTimerText).text = $"부활 ({respawnTime - elapsedTime})";
            await UniTask.Delay(ONE_SECOND);
            ++elapsedTime;
        }
    }

    public void RefreshProfile()
    {
        foreach (UI_Profile profileItem in _profiles)
        {
            profileItem.RefreshUpdatedInGameItems();
        }
    }
}
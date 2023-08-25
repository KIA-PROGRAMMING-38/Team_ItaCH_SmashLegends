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

    private List<UI_ProfileItem> _profiles = new List<UI_ProfileItem>();

    public override void Init()
    {
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindObject(typeof(GameObjects));
        Bind<UI_ProfileItem>(typeof(UIProfiles));

        PopulateProfile();

        GetObject((int)GameObjects.PlayerRespawnTimer).SetActive(false);
        RefreshPopupUI();

        Managers.StageManager.OnTimeChange -= RefreshGameTimer;
        Managers.StageManager.OnTimeChange += RefreshGameTimer;
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

        for (int team = 0; team < maxTeamCount; ++team)
        {
            CreateProfileItem(team, parentObject);
        }
    }

    public void CreateProfileItem(int team, GameObject parentObject)
    {
        UI_ProfileItem profileItem = Managers.UIManager.MakeSubItem<UI_ProfileItem>(parentObject.transform);

        profileItem.SetInfo(team);
        _profiles.Add(profileItem);
    }

    private void RefreshPopupUI()
    {
        RefreshGameTimer(Managers.StageManager.RemainGameTime);
        RefreshProfileItem();
    }

    private void RefreshGameTimer(int remainTime)
    {
        GetText((int)Texts.GameTimerText).text = $"{remainTime / 60:D2}:{remainTime % 60:D2}";
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
        await GetImage((int)Images.PlayerRespawnTimerFill).ChangeFillAmountGradually(MAX_FILL_AMOUNT, respawnTime);
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

    public void RefreshProfileItem()
    {
        foreach (UI_ProfileItem profileItem in _profiles)
        {
            profileItem.RefreshUI();
        }
    }
}
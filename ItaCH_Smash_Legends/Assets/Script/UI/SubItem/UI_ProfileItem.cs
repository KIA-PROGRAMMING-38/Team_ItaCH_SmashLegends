using UnityEngine;

public class UI_ProfileItem : UIBase
{
    enum ProfileSubItemObjects
    {
        HpBar,
        Portrait,
        ScoreSet
    }

    private UserData _userData;
    private UI_Portrait _portrait;

    public override void Init()
    {
        BindObject(typeof(ProfileSubItemObjects));
    }

    public void SetInfo(TeamType teamType)
    {
        int teamTypeIndex = (int)teamType - 1; // 0 = TeamType.None 제외
        _userData = Managers.StageManager.CurrentGameMode.Teams[teamTypeIndex].Members[0];


        _portrait = Utils.GetOrAddComponent<UI_Portrait>(GetObject((int)ProfileSubItemObjects.Portrait));
        _portrait.SetInfo(_userData);

        RefreshUI();
    }

    public void RefreshUI()
    {
        // To Do : 각 SubItem Refresh
    }
}

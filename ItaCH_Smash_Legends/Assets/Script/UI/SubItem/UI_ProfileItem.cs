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

    public void SetInfo(int teamIndex)
    {
        _userData = Managers.StageManager.CurrentGameMode.Teams[teamIndex].Members[0];

        _portrait = Utils.GetOrAddComponent<UI_Portrait>(GetObject((int)ProfileSubItemObjects.Portrait));
        _portrait.SetInfo(_userData);

        if (_userData.TeamType == TeamType.Red)
        {
            this.GetComponent<RectTransform>().FlipY();
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        // To Do : 각 SubItem Refresh
    }
}

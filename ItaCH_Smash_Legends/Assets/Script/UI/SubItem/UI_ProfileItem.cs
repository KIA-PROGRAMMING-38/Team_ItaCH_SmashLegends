using UnityEngine;

public class UI_ProfileItem : UIBase
{
    enum ProfileSubItemObjects
    {
        HpBar,
        Portrait,
        ScoreSet
    }

    private Team _team;
    private UserData _userData;
    private UI_Portrait _portrait;
    private UI_HpBar _hpBar;
    private UI_ScoreSet _scoreSet;

    public override void Init()
    {
        BindObject(typeof(ProfileSubItemObjects));
    }

    public void SetInfo(int teamIndex)
    {
        _team = Managers.StageManager.CurrentGameMode.Teams[teamIndex];
        _userData = _team.Members[0];

        _portrait = Utils.GetOrAddComponent<UI_Portrait>(GetObject((int)ProfileSubItemObjects.Portrait));
        _portrait.SetInfo(_userData);

        _hpBar = Utils.GetOrAddComponent<UI_HpBar>(GetObject((int)ProfileSubItemObjects.HpBar));
        _hpBar.SetInfo(_userData);

        _scoreSet = Utils.GetOrAddComponent<UI_ScoreSet>(GetObject((int)ProfileSubItemObjects.ScoreSet));
        _scoreSet.SetInfo(_team);

        if (_userData.TeamType == TeamType.Red)
        {
            this.GetComponent<RectTransform>().FlipY();
        }
    }

    public void RefreshUI()
    {
        _hpBar.RefreshUI(_userData.OwnedLegend.HPRatio);
        _scoreSet.RefreshScoreSet(_userData.TeamType);
    }
}

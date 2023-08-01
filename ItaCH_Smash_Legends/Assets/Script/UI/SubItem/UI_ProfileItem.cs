using UnityEngine;

public class UI_ProfileItem : UIBase
{
    enum ProfileSubItemObjects
    {
        HpBar,
        Portrait,
        ScoreSet
    }

    private bool _isInitialSet = true;

    private UserData _userData;
    private UI_Portrait _portrait;

    public override void Init()
    {
        if (_isInitialSet == false)
        {
            return;
        }

        BindObject(typeof(ProfileSubItemObjects));

        _isInitialSet = false;
    }

    public void SetInfo(int teamIndex)
    {
        if (_isInitialSet)
        {
            Init();
        }

        _userData = Managers.StageManager.CurrentGameMode.Teams[teamIndex].Members[0];


        _portrait = Utils.GetOrAddComponent<UI_Portrait>(GetObject((int)ProfileSubItemObjects.Portrait));
        _portrait.SetInfo(_userData, true);

        RefreshUI();
    }

    public void RefreshUI()
    {
        // To Do : 각 SubItem Refresh
    }
}

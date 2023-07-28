using UnityEngine;

public class UI_Profile : UIBase
{
    enum GameObjects
    {
        HpBar,
        Portrait,
        ScoreSet
    }

    private UserData userData;

    public override void Init()
    {
        BindObject(typeof(GameObjects));

        RefreshOnceUpdatedItems();
        RefreshUpdatedInGameItems();
    }

    public void SetInfo(int teamIndex)
    {
        //userData = Managers.StageManager.CurrentGameMode.Teams[teamIndex].Members[0];
        RefreshOnceUpdatedItems();
        RefreshUpdatedInGameItems();
    }

    public void RefreshOnceUpdatedItems()
    {
        // To Do : Refresh Portrait.UserName / Portrait.LegendFaceImage
    }

    public void RefreshUpdatedInGameItems()
    {
        // To Do : Refresh HpBar / Portrait.RespawnTimer / ScoreSet
    }
}

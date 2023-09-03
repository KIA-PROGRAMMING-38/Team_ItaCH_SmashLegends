using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ScoreSet : UIBase
{
    enum GameObjects
    {
        ScoreFrame
    }

    List<UI_ScoreSetSubItem> _scores = new List<UI_ScoreSetSubItem>();
    private Team _team;

    public override void Init()
    {
        BindObject(typeof(GameObjects));

        PopulateScoreSetSubItem();
    }

    public void PopulateScoreSetSubItem()
    {
        _scores.Clear();

        foreach (Transform child in this.transform)
        {
            Managers.ResourceManager.Destroy(child.gameObject);
        }

        int maxCount = Managers.StageManager.CurrentGameMode.WinningScore;

        for (int scoreItemIndex = 0; scoreItemIndex < maxCount; ++scoreItemIndex)
        {
            CreateScoreSetSubItem(scoreItemIndex);
        }
    }

    public void CreateScoreSetSubItem(int index)
    {
        UI_ScoreSetSubItem scoreSubItem = Managers.UIManager.MakeSubItem<UI_ScoreSetSubItem>(this.transform);
        scoreSubItem.SetInfo(index);

        _scores.Add(scoreSubItem);
    }

    public void SetInfo(Team team)
    {
        _team = team;
        RefreshScoreSet(_team.Type);
    }

    public void RefreshScoreSet(TeamType teamType)
    {
        if (_team.Score == 0)
        {
            return;
        }
        int index = _team.Score - 1;
        _scores[index].ActivateScoreSetSubItem(teamType);
    }
}

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
    private int _currentScore;

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

    public void SetInfo(TeamType teamType, int currentScore) // TO DO : 점수 획득 시 UI_ProfileItem Refresh 하면서 호출
    {
        if (_currentScore == currentScore)
        {
            return;
        }
        _currentScore = currentScore;
        RefreshScoreSet(teamType);
    }

    public void RefreshScoreSet(TeamType teamType)
    {
        int index = _currentScore - 1;
        _scores[index].ActivateScoreSetSubItem(teamType);
    }
}

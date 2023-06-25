using UnityEngine;
using UnityEngine.UI;

public class ScoreSet : MonoBehaviour
{
    private Image[] _fillings;
    private TeamType _teamType;

    public void InitScoreSetSettings(TeamType teamType)
    {
        int numberOfScoreSet = transform.childCount;
        _teamType = teamType;
        _fillings = new Image[numberOfScoreSet];
        for (int i = 0; i < numberOfScoreSet; ++i)
        {
            _fillings[i] = transform.GetChild(i).GetChild(0).GetComponent<Image>();
        }
    }
    public void GetScore(int currentScore, TeamType teamType)
    {
        if (teamType.Equals(_teamType))
        {
            _fillings[currentScore - 1].enabled = true;
        }
    }
}

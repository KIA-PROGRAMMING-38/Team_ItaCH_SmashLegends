using System;
using System.Collections.Generic;

public class GameModeBase
{
    private GameModeType _type;

    public event Action<Team?> OnNotifyWinnerTeam;

    private List<Team> _teams;

    public void Init()
    {

    }

    public void IsOver()
    {
        switch (_type)
        {
            case GameModeType.Duel:
                JudgeWinnerOnDuel();

                break;

            case GameModeType.TeamMatch:
                JudgeWinnerOnTeamMatch();

                break;

            default: // 미구현 오류
                break;
        }
    }

    Team? GetWinnerTeam(List<Team> teams)
    {
        var redTeamType = teams[0].GetType();
        var blueTeamType = teams[1].GetType();


        if (teams[0].Score == teams[1].Score)
        {
            return null;
        }

        if (teams[0].Score > teams[1].Score)
        {
            return teams[0];
        }
        else
        {
            return teams[1];
        }
    }

    private const Team? DRAW = null;

    public void JudgeWinnerOnDuel()
    {
        Team? winnerTeam = GetWinnerTeam(_teams);

        if (winnerTeam is not null)
        {
            OnNotifyWinnerTeam?.Invoke(winnerTeam);

            return;
        }

        var redTeamHpRatio = _teams[0].Members[0].OwnedLegend.HPRatio;
        var blueTeamHpRatio = _teams[1].Members[0].OwnedLegend.HPRatio;

        if (redTeamHpRatio == blueTeamHpRatio)
        {
            OnNotifyWinnerTeam?.Invoke(DRAW);

            return;
        }

        if (redTeamHpRatio > blueTeamHpRatio)
        {
            OnNotifyWinnerTeam?.Invoke(_teams[0]);
        }
        else
        {
            OnNotifyWinnerTeam?.Invoke(_teams[1]);
        }
    }

    public void JudgeWinnerOnTeamMatch()
    {
        Team? winnerTeam = GetWinnerTeam(_teams);

        if (winnerTeam is not null)
            OnNotifyWinnerTeam?.Invoke(winnerTeam);

        else
            OnNotifyWinnerTeam?.Invoke(DRAW);
    }
}
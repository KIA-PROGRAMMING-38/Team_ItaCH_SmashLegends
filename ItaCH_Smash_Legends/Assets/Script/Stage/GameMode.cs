using System;
using System.Collections.Generic;
using UnityEngine;

public class GameMode // TO DO : 모드 추가 시 추상 클래스 정의 및 해당 클래스 상속
{
    // TO DO : 모드 추가 시 ModeData 추가 및 DataManager로 접근
    public GameModeType GameModeType { get => _currentGameModeType; set => _currentGameModeType = value; }
    public string Map { get; private set; }
    public int MaxPlayer { get => _totalPlayer; }
    public int MaxTeamCount { get => _maxTeamCount; }
    public int MaxTeamMember { get => _maxTeamMember; }
    public int MaxGameTime { get => _maxGameTimeSec; }
    public int WinningScore { get => _winningScore; }
    public float ModeDefaultRespawnTime { get => _modeDefaultRespawnTime; }

    private GameModeType _currentGameModeType;

    private int _totalPlayer;
    private int _maxTeamCount;
    private int _maxTeamMember;
    private int _winningScore;
    private int _maxGameTimeSec;

    private float _modeDefaultRespawnTime;
    public List<Team> Teams { get; set; }

    public event Action<TeamType> OnNotifyWinnerTeam;

    private const Team? DRAW = null;
    public void Init(GameModeType gameModeType)
    {
        GetGameModeData(gameModeType);
        Teams = new List<Team>();

        Managers.LobbyManager.OnUpdatePlayerList -= SetUserTeam;
        Managers.LobbyManager.OnUpdatePlayerList += SetUserTeam;
    }

    private void GetGameModeData(GameModeType gameModeType)
    {
        // TO DO : DataManager에서 가져오도록
        _currentGameModeType = gameModeType;
        _totalPlayer = 2;
        _maxTeamCount = 2;
        _maxTeamMember = 1;
        _maxGameTimeSec = 120;
        _winningScore = 3;
        _modeDefaultRespawnTime = 5f;
        Map = StringLiteral.MAP_PREFAB_PATH;
        // 현재 Duel Mode 값 직접 지정
    }

    public void SetUserTeam(UserData user)
    {
        Team team = GetTeamAvailable();
        team.AddMember(user);
    }

    private Team GetTeamAvailable()
    {
        if (IsNewTeamNeeded(Teams, MaxTeamMember))
        {
            Team newTeam = new Team();
            newTeam.Init();
            Teams.Add(newTeam);
            newTeam.Type = (Teams.Count == 1) ? TeamType.Blue : TeamType.Red; // 자신이 속한 팀을 제외한 모든 팀은 red team으로 두어 레이어 구분 및 피격 판정
            return newTeam;
        }
        else
        {
            return Teams[^1];
        }

        static bool IsNewTeamNeeded(List<Team> teams, int max) => teams.Count == 0 || teams[^1].Members.Count == max;
    }

    public void IsOver()
    {
        switch (_currentGameModeType)
        {
            case GameModeType.Duel:
                JudgeWinnerOnDuel();

                break;

            case GameModeType.TeamMatch:
                //JudgeWinnerOnTeamMatch(); // To Do : Match 모드 구현 시 추가
                break;

            default:

                throw new NotImplementedException($"Error with current mode type : {_currentGameModeType} is not implemented.");
        }
    }

    private Team? GetWinnerTeam(List<Team> teams)
    {

        if (teams[(int)TeamType.Blue].Score == teams[(int)TeamType.Red].Score)
        {
            return null;
        }

        if (teams[(int)TeamType.Blue].Score > teams[(int)TeamType.Red].Score)
        {
            return teams[(int)TeamType.Blue];
        }
        else
        {
            return teams[(int)TeamType.Red];
        }
    }

    private void JudgeWinnerOnDuel()
    {
        Team? winnerTeam = GetWinnerTeam(Teams);

        if (winnerTeam is not null)
        {
            OnNotifyWinnerTeam?.Invoke(winnerTeam.Type);

            return;
        }

        float blueTeamHpRatio = Teams[(int)TeamType.Blue].Members[0].OwnedLegend.HPRatio;
        float redTeamHpRatio = Teams[(int)TeamType.Red].Members[0].OwnedLegend.HPRatio;

        if (redTeamHpRatio == blueTeamHpRatio)
        {
            OnNotifyWinnerTeam?.Invoke(TeamType.None);

            return;
        }

        if (redTeamHpRatio > blueTeamHpRatio)
        {
            OnNotifyWinnerTeam?.Invoke(TeamType.Red);
        }
        else
        {
            OnNotifyWinnerTeam?.Invoke(TeamType.Blue);
        }
    }
}

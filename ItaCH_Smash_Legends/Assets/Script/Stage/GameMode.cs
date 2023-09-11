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
        Team defaultTeam = new Team();
        defaultTeam.InitDefaultTeam();
        Teams.Add(defaultTeam);

        Managers.LobbyManager.OnUpdatePlayerList -= SetUserTeam;
        Managers.LobbyManager.OnUpdatePlayerList += SetUserTeam;

        OnNotifyWinnerTeam -= Managers.StageManager.EndGame;
        OnNotifyWinnerTeam += Managers.StageManager.EndGame;
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
        Map = StringLiteral.DUEL_MODE_MAP_PREFAB_PATH;
        // 현재 Duel Mode 값 직접 지정
    }

    public void SetUserTeam(UserData user)
    {
        Team team = GetTeamAvailable();
        team.AddMember(user);
        Managers.UIManager.FindPopup<UI_MatchingPopup>().RefreshUI();
    }

    private Team GetTeamAvailable()
    {
        if (IsNewTeamNeeded(Teams, MaxTeamMember))
        {
            Team newTeam = new Team();
            newTeam.Init();
            Teams.Add(newTeam);
            return newTeam;
        }
        else
        {
            return Teams[^1];
        }

        static bool IsNewTeamNeeded(List<Team> teams, int max) => teams.Count == 0 || teams[^1].Members.Count == max;
    }

    public void SetModeUI() // TO DO : 추상클래스에서는 가상함수로 정의하고 각 모드가 재정의하는 구조
    {
        Managers.UIManager.ShowPopupUI<UI_DuelModePopup>().Init();
    }

    public void IsOver()
    {
        Managers.SoundManager.Play(SoundType.SFX, StringLiteral.SFX_MATCH_OVER);

        switch (_currentGameModeType)
        {
            case GameModeType.Duel:
                JudgeWinnerOnDuel();
                break;

            case GameModeType.TeamMatch:
                Team winnerTeam = GetWinnerTeam(Teams);
                OnNotifyWinnerTeam?.Invoke(winnerTeam.Type);
                break;

            default:

                Debug.LogError($"Error with current mode type : {_currentGameModeType} is not implemented.");
                break;
        }
    }

    private Team? GetWinnerTeam(List<Team> teams)
    {
        if (teams[(int)TeamType.Blue].Score == teams[(int)TeamType.Red].Score)
        {
            return DRAW;
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

        if (winnerTeam is not DRAW)
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

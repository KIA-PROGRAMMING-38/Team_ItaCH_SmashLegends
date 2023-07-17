using System.Collections.Generic;
using UnityEngine;

public class GameMode // TO DO : 모드 추가 시 추상 클래스 정의 및 해당 클래스 상속
{
    // TO DO : 모드 추가 시 ModeData 추가 및 DataManager로 접근
    public GameModeType GameModeType { get => _currentGameModeType; set => _currentGameModeType = value; }
    public GameObject Map { get => _currentMap; }
    public int MaxPlayer { get => _totalPlayer; }
    public int MaxTeamCount { get => _maxTeamCount; }
    public int MaxTeamMember { get => _maxTeamMember; }
    public int MaxGameTime { get => _maxGameTimeSec; }
    public int WinningScore { get => _winningScore; }
    public Transform[] SpawnPoints { get => _spawnPoints; }
    public float ModeDefaultRespawnTime { get => _modeDefaultRespawnTime; }

    private GameModeType _currentGameModeType;
    private GameObject _currentMap;
    private int _totalPlayer;
    private int _maxTeamCount;
    private int _maxTeamMember;
    private int _winningScore;
    private int _maxGameTimeSec;
    private float _modeDefaultRespawnTime;
    private Transform[] _spawnPoints;

    public void Init(GameModeType gameModeType)
    {
        GetGameModeData(gameModeType);
        GetMapData();
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
        // 현재 Duel Mode 값 직접 지정
    }
    private void GetMapData() 
    {        
        _currentMap = Resources.Load<GameObject>(StringLiteral.MAP_PREFAB_PATH);
        _spawnPoints = _currentMap.transform.Find(StringLiteral.SPAWN_POINTS).GetComponentsInChildren<Transform>();
    }

    public TeamType GetWinningTeam(in List<Team> teams)
    {
        // 더 높은 점수를 가진 팀 승리
        // 동점이면 더 높은 체력 비율을 가진 팀 승리
        // 

        return TeamType.None;
    }
}

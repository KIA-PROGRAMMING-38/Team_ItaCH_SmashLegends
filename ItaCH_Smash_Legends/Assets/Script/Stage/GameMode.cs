using UnityEngine;

public class GameMode
{
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
        // 추후 데이터 분리 필요
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
        string mapPrefabPath = "Map/SingleLogBridge/Prefab/Map"; // 추후 데이터 분리 필요
        _currentMap = Resources.Load<GameObject>(mapPrefabPath);
        _spawnPoints = _currentMap.transform.GetChild(0).GetComponentsInChildren<Transform>();
    }
}

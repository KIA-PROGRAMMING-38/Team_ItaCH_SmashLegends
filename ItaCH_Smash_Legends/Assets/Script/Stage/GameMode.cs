using UnityEngine;

public class GameMode
{
    public GameModeType GameModeType { get => _currentGameModeType; set => _currentGameModeType = value; }
    public GameObject Map { get => _currentMap; private set => _currentMap = value; }
    public int TotalPlayer { get => _totalPlayer; private set => _totalPlayer = value; }
    public int TeamSize { get => _teamSize; private set => _teamSize = value; }
    public int MaxGameTime { get => _maxGameTimeSec; private set => _maxGameTimeSec = value; }
    public int WinningScore { get => _winningScore; private set => _winningScore = value; }
    public Transform[] SpawnPoints { get => _spawnPoints; private set => _spawnPoints = value; }
    public float ModeDefaultRespawnTime { get => _modeDefaultRespawnTime; private set => _modeDefaultRespawnTime = value; }

    private GameModeType _currentGameModeType;
    private GameObject _currentMap;
    private int _totalPlayer;
    private int _teamSize;
    private int _winningScore;
    private int _maxGameTimeSec;
    private float _modeDefaultRespawnTime;
    private Transform[] _spawnPoints;

    public void InitGameMode(GameModeType gameModeType)
    {
        GetGameModeData(gameModeType);
        GetMapData();
    }
    private void GetGameModeData(GameModeType gameModeType)
    {
        // 추후 데이터 분리 필요
        _currentGameModeType = gameModeType;
        _totalPlayer = 2;
        _teamSize = 1;
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

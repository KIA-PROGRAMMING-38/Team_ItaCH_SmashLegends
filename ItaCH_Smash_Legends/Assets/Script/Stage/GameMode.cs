using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameMode
{
    private GameModeType _currentGameModeType;
    private GameObject _currentMap;
    private int _totalPlayer;
    private int _winningScore;
    private float _maxGameTime;
    private Transform[] _spawnPoints;
    public GameModeType GameModeType { get => _currentGameModeType; set => _currentGameModeType = value; }
    public GameObject Map { get => _currentMap; private set => _currentMap = value; }
    public int TotalPlayer { get => _totalPlayer; private set => _totalPlayer = value; }
    public float MaxGameTime { get => _maxGameTime; private set => _maxGameTime = value; }
    public int WinningScore { get => _winningScore; private set => _winningScore = value; }
    public Transform[] SpawnPoints { get => _spawnPoints; private set => _spawnPoints = value; }
    public void InitGameMode(GameModeType gameModeType)
    {
        GetGameModeData(gameModeType);
        GetMapData();
    }
    private void GetGameModeData(GameModeType gameModeType)
    {
        // 추후 데이터 분리 필요
        _totalPlayer = 2;
        _maxGameTime = 120f;
        _winningScore = 3;
        // 현재 Duel Mode 값 직접 지정
    }
    private void GetMapData()
    {
        string mapPrefabPath = "Map/SingleLogBridge/Prefab/Map"; // 추후 데이터 분리 필요
        _currentMap = Resources.Load<GameObject>(mapPrefabPath);
        _spawnPoints = _currentMap.transform.GetChild(0).GetComponentsInChildren<Transform>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{      
    private GameMode _currentGameMode;
    private GameModeType _selectedGameMode = GameModeType.Duel;
    // 현재는 테스트를 위해 직접 값 대입, 향후 방장이 선택한 값 연결
    private GameObject _characterPrefab;
    private GameObject _1PCharacter;
    private GameObject _2PCharacter;
    private string _characterPrefabPath;
    private Transform[] _spawnPoints;
    private Transform _1pSpawnPoint;
    private Transform _2pSpawnPoint;

    private void Start()
    {        
        _spawnPoints = GameObject.FindWithTag("SpawnPoints").GetComponentsInChildren<Transform>();
        _1pSpawnPoint = _spawnPoints[1];        
        _2pSpawnPoint = _spawnPoints[2];        
        StartGame();
    }    
    public void StartGame()
    {
        _characterPrefabPath = "Charater/Peter/Peter_Ingame/Peter_Ingame"; // 캐릭터 선택 기능 구현 시 캐릭터 이름으로 경로 구성 필요
        _characterPrefab = Resources.Load<GameObject>(_characterPrefabPath);
        if (_characterPrefab != null)
        {
            _1PCharacter = Instantiate(_characterPrefab, _1pSpawnPoint);            
            _2PCharacter = Instantiate(_characterPrefab, _2pSpawnPoint);
        }
        else
        {
            Debug.LogError("Failed to load the prefab at path: " + _characterPrefabPath);
        }        
        // InGame Scene 불러와야 함.
        // _currentGameMode.Map의 게임모드 타입과 연결되는 맵 프리펩 불러와 Instanciate   
    }
    public void InitGameMode(GameModeType gameModeSelected)
    {
        if(_currentGameMode == null)
        {
            _currentGameMode = new GameMode();
        }
        _currentGameMode.GameModeType = gameModeSelected;
        // 게임 모드 타입과 일치하는 게임 모드 로직 load 필요

    }
}

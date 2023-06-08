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

    // 테스트를 위해 인스펙터 창을 사용. 추후 리소스 폴더에서 로드하는 것으로 변경;
    [SerializeField] private GameObject _legendUIPrefab;
    [SerializeField] private GameObject[] _modeUIPrefab;
    private List<GameObject> _legendUI;
    private GameObject _modeUI;
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
        SetModeUI(_selectedGameMode);
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

    public void SetModeUI(GameModeType gameModeType)
    {
        switch (gameModeType)
        {
            case GameModeType.None:
                Debug.Log("Failed to Find ModeUI" + $"{gameModeType}");
                break;
            case GameModeType.Duel:
                _legendUI = new List<GameObject>();
                SetLegendUI(_1PCharacter);
                SetLegendUI(_2PCharacter);
                _modeUI = Instantiate(_modeUIPrefab[(int)GameModeType.Duel]);
                //추후 스테이지에 존재하는 레전드를 하나로 관리하는 배열 생성하여 foreach로 생성.
                break;
            case GameModeType.TeamMatch:
                // 듀얼과 유사한 로직으로 구현
                Debug.Log("Failed to Find ModeUI" + $"{gameModeType}");
                break;
        }
    }
    public void SetLegendUI(GameObject player)
    {
        _legendUI.Add(Instantiate(_legendUIPrefab, player.transform));
    }
}

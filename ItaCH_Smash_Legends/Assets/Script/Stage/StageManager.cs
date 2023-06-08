using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{      
    private GameMode _currentGameMode;
    private GameModeType _selectedGameMode = GameModeType.Duel;
    // ����� �׽�Ʈ�� ���� ���� �� ����, ���� ������ ������ �� ����
    private GameObject _characterPrefab;
    private GameObject _1PCharacter;
    private GameObject _2PCharacter;
    private string _characterPrefabPath;
    private Transform[] _spawnPoints;
    private Transform _1pSpawnPoint;
    private Transform _2pSpawnPoint;

    // �׽�Ʈ�� ���� �ν����� â�� ���. ���� ���ҽ� �������� �ε��ϴ� ������ ����;
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
        _characterPrefabPath = "Charater/Peter/Peter_Ingame/Peter_Ingame"; // ĳ���� ���� ��� ���� �� ĳ���� �̸����� ��� ���� �ʿ�
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
        // InGame Scene �ҷ��;� ��.
        // _currentGameMode.Map�� ���Ӹ�� Ÿ�԰� ����Ǵ� �� ������ �ҷ��� Instanciate
        SetModeUI(_selectedGameMode);
    }
    public void InitGameMode(GameModeType gameModeSelected)
    {
        if(_currentGameMode == null)
        {
            _currentGameMode = new GameMode();
        }
        _currentGameMode.GameModeType = gameModeSelected;
        // ���� ��� Ÿ�԰� ��ġ�ϴ� ���� ��� ���� load �ʿ�

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
                //���� ���������� �����ϴ� �����带 �ϳ��� �����ϴ� �迭 �����Ͽ� foreach�� ����.
                break;
            case GameModeType.TeamMatch:
                // ���� ������ �������� ����
                Debug.Log("Failed to Find ModeUI" + $"{gameModeType}");
                break;
        }
    }
    public void SetLegendUI(GameObject player)
    {
        _legendUI.Add(Instantiate(_legendUIPrefab, player.transform));
    }
}

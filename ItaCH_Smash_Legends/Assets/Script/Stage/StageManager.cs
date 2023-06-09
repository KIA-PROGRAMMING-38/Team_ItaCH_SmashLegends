using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using System.Text;
using UnityEngine.InputSystem;

public class StageManager : MonoBehaviour
{
    private const GameModeType DEFAULT_GAME_MODE = GameModeType.Duel;
    private GameMode _currentGameMode;
    private GameModeType _selectedGameMode;
    private GameObject[] _playerCharacterInstances;
    public GameObject[] Players { get => _playerCharacterInstances; }
    private GameObject[] _teamBlueCharacter;
    private GameObject[] _teamRedCharacter;
    private const int INDEX_OFFSET_FOR_ZERO = 1;
    private int _totalPlayer;
    private int _teamSize;
    private int _teamMemberIndex;
    private int _teamBlueScore;
    private int _teamRedScore;
    private int _winningScore;


    private GameObject _legendUIPrefab;
    private GameObject[] _modeUIPrefab;

    private List<GameObject> _legendUI;
    private GameObject _modeUI;

    public event Action<int, TeamType> OnTeamScoreChanged;
    private void Awake()
    {
        _selectedGameMode = DEFAULT_GAME_MODE;
        GetGameMode(_selectedGameMode);
    }
    private void Start()
    {
        SetStage(_currentGameMode);
    }
    public void GetGameMode(GameModeType gameModeSelected)
    {
        if (_currentGameMode == null)
        {
            _currentGameMode = new GameMode();
        }
        _currentGameMode.InitGameMode(gameModeSelected);
        _totalPlayer = _currentGameMode.TotalPlayer;
        _teamSize = _currentGameMode.TeamSize;
        _winningScore = _currentGameMode.WinningScore;
    }
    public void SetStage(GameMode currentGameMode)
    {
        CreateMap(currentGameMode);
        _playerCharacterInstances = new GameObject[_totalPlayer + INDEX_OFFSET_FOR_ZERO];
        _teamBlueCharacter = new GameObject[_teamSize];
        _teamRedCharacter = new GameObject[_teamSize];
        for (int playerID = 1; playerID <= _totalPlayer; playerID++)
        {
            CreateCharacter(playerID, currentGameMode.SpawnPoints);
        }
        SetModeUI(currentGameMode.GameModeType);
    }
    public void CreateCharacter(int playerID, Transform[] spawnPoints) // 캐릭터 선택 기능 구현 시 매개변수로 선택한 캐릭터 함께 전달
    {
        string characterPrefabPath = "Charater/Peter/Peter_Ingame/Peter_Ingame"; // 캐릭터 선택 기능 구현 시 캐릭터 이름으로 경로 구성 필요
        GameObject characterPrefab = Resources.Load<GameObject>(characterPrefabPath);
        if (characterPrefab != null)
        {
            if (spawnPoints[playerID] != null)
            {
                GameObject characterInstance = Instantiate(characterPrefab, spawnPoints[playerID].position, Quaternion.identity);
                SetTeam(characterInstance, playerID);
                SetPlayerInputController(characterInstance, playerID);
                _playerCharacterInstances[playerID] = characterInstance;
            }
            else
            {
                Debug.LogError(playerID + "P character spawn position is Null");
            }
        }
        else
        {
            Debug.LogError("Failed to load the prefab at path: " + characterPrefabPath);
        }
    }
    public void SetTeam(GameObject character, int id)
    {
        CharacterStatus characterStatus = character.GetComponent<CharacterStatus>();
        characterStatus.PlayerID = id;
        if (id > _teamSize)
        {
            characterStatus.TeamType = TeamType.Red;
            _teamMemberIndex = id - _teamSize - INDEX_OFFSET_FOR_ZERO;
            _teamRedCharacter[_teamMemberIndex] = character;
            characterStatus.TeamSpawnPoint = characterStatus.transform.position;
        }
        else
        {
            characterStatus.TeamType = TeamType.Blue;
            _teamMemberIndex = id - INDEX_OFFSET_FOR_ZERO;
            _teamBlueCharacter[_teamMemberIndex] = character;
            characterStatus.TeamSpawnPoint = characterStatus.transform.position;
        }
        characterStatus.OnPlayerDie -= UpdateTeamScore;
        characterStatus.OnPlayerDie += UpdateTeamScore;
    }
    public void SetPlayerInputController(GameObject character, int id)
    {
        UnityEngine.InputSystem.PlayerInput playercontroller;
        switch (id)
        {
            case 1:
                playercontroller = character.GetComponent<UnityEngine.InputSystem.PlayerInput>();
                playercontroller.defaultActionMap = "FirstPlayerActions";
                break;
            case 2:
                playercontroller = character.GetComponent<UnityEngine.InputSystem.PlayerInput>();
                playercontroller.defaultActionMap = "SecondPlayerActions";
                break;
            default:
                return;
        }
    }
    public void CreateMap(GameMode gameMode)
    {
        GameObject mapInstance = Instantiate(gameMode.Map);
    }
    public void ChangeGameMode(GameModeType gameModeSelected)
    {
        if (_selectedGameMode == gameModeSelected)
        {
            return;
        }
        else
        {
            GetGameMode(gameModeSelected);
        }
    }

    public void SetModeUI(GameModeType gameModeType)
    {
        _modeUIPrefab = new GameObject[Enum.GetValues(typeof(GameModeType)).Length];
        StringBuilder stringBuilder = new StringBuilder();
        string ModeUIFolderPath = "UI/ModeUI/ModeUI_";
        for (int i = 0; i < _modeUIPrefab.Length; ++i)
        {
            stringBuilder.Clear();
            stringBuilder.Append(ModeUIFolderPath);
            stringBuilder.Append($"{i:00}");
            _modeUIPrefab[i] = Resources.Load<GameObject>(stringBuilder.ToString());
        }

        switch (gameModeType)
        {
            case GameModeType.None:
                Debug.Log("Failed to Find ModeUI" + $"{gameModeType}");
                break;
            case GameModeType.Duel:
                _legendUI = new List<GameObject>();
                for (int i = 1; i <= _totalPlayer; ++i)
                {
                    SetLegendUI(_playerCharacterInstances[i]);
                }
                _modeUI = Instantiate(_modeUIPrefab[(int)GameModeType.Duel]);
                _modeUI.GetComponent<ModeUI>().InitModeUISettings(this);
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
        _legendUIPrefab = Resources.Load<GameObject>("UI/LegendUI");
        GameObject legendUI = Instantiate(_legendUIPrefab);
        legendUI.GetComponent<LegendUI>().InitLegendUISettings(player.transform);
        _legendUI.Add(legendUI);
    }
    private void UpdateTeamScore(CharacterStatus character)
    {
        if (character.TeamType == TeamType.Blue)
        {
            ++_teamRedScore;
            OnTeamScoreChanged.Invoke(_teamRedScore, TeamType.Red);
        }
        else
        {
            ++_teamBlueScore;
            OnTeamScoreChanged.Invoke(_teamBlueScore, TeamType.Blue);
        }        
        if (_teamBlueScore == _winningScore || _teamRedScore == _winningScore)
        {
            EndGameMode();
        }
    }
    private int GetTeamScore(TeamType team)
    {
        return (team == TeamType.Blue) ? _teamBlueScore : _teamRedScore;
    }
    private void EndGameMode()
    {
        int teamBlueEndScore = GetTeamScore(TeamType.Blue);
        int teamRedEndScore = GetTeamScore(TeamType.Red);
        TeamType winningTeam = TeamType.None;
        if (teamBlueEndScore == teamRedEndScore)
        {
            CheckTeamHealthRatio();
        }
        else
        {
            winningTeam = (teamBlueEndScore > teamRedEndScore) ? TeamType.Blue : TeamType.Red;
        }
    }
    private TeamType CheckTeamHealthRatio()
    {
        // 다음 이슈에서 구현
        return TeamType.None;
    }
}

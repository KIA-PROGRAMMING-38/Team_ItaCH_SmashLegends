using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

public class StageManager : MonoBehaviour
{
    private const GameModeType DEFAULT_GAME_MODE = GameModeType.Duel;
    private GameMode _currentGameMode;
    private GameModeType _selectedGameMode;
    private GameObject[] _playerCharacterInstances;
    public GameObject[] Players { get => _playerCharacterInstances; }
    private CharacterStatus[] _teamBlueCharacter;
    private CharacterStatus[] _teamRedCharacter;
    private const int INDEX_OFFSET_FOR_ZERO = 1;
    private int _totalPlayer;
    private int _teamSize;
    private int _teamMemberIndex;
    private int _teamBlueScore;
    private int _teamRedScore;
    private int _winningScore;

    private float _modeDefaultRespawnTime;
    private float _gameTime;
    private int _gameTimeInt;
    public float GameTime
    {
        get => _gameTime;
        set
        {
            _gameTime = value;
            int currentGameTimeInt = Mathf.FloorToInt(_gameTime);
            if (_gameTimeInt != currentGameTimeInt)
            {
                _gameTimeInt = currentGameTimeInt;
                OnTimeChange.Invoke(RemainGameTime);
            }
        }
    }
    public int RemainGameTime => Mathf.Max(_currentGameMode.MaxGameTime - _gameTimeInt, 0);
    private bool _isGameOver;

    private GameObject _legendUIPrefab;
    private GameObject[] _modeUIPrefab;

    private List<GameObject> _legendUI;
    private GameObject _modeUI;

    public event Action<int, TeamType> OnTeamScoreChanged;
    public event Action<int> OnTimeChange;

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
        _modeDefaultRespawnTime = _currentGameMode.ModeDefaultRespawnTime;
        _isGameOver = false;
    }
    public void SetStage(GameMode currentGameMode)
    {
        CreateMap(currentGameMode);
        _playerCharacterInstances = new GameObject[_totalPlayer + INDEX_OFFSET_FOR_ZERO];
        _teamBlueCharacter = new CharacterStatus[_teamSize];
        _teamRedCharacter = new CharacterStatus[_teamSize];
        for (int playerID = 1; playerID <= _totalPlayer; playerID++)
        {
            CreateCharacter(playerID, currentGameMode.SpawnPoints);
        }
        SetModeUI(currentGameMode.GameModeType);
        StartCoroutine(UpdateGameTime());
    }
    public void CreateCharacter(int playerID, Transform[] spawnPoints) // 캐릭터 선택 기능 구현 시 매개변수로 선택한 캐릭터 함께 전달
    {
        string characterPrefabPath = "Charater/Peter/Peter_Ingame/Peter_Ingame"; // 캐릭터 선택 기능 구현 시 캐릭터 이름으로 경로 구성 필요
        string hookPrefabPath = "Charater/Hook/Hook_Ingame/Hook_Ingame"; // 캐릭터 선택 기능 구현 시 캐릭터 이름으로 경로 구성 필요
        string alicePrefabPath = "Charater/Alice/Alice_Ingame/Alice_Ingame"; // 캐릭터 선택 기능 구현 시 캐릭터 이름으로 경로 구성 필요
        GameObject characterPrefab = Resources.Load<GameObject>(characterPrefabPath);
        GameObject hookPrefab = Resources.Load<GameObject>(hookPrefabPath);
        GameObject alicePrefab = Resources.Load<GameObject>(alicePrefabPath);

        if (characterPrefab != null)
        {
            if (spawnPoints[playerID] != null)
            {
                if (playerID == 2)
                {
                    characterPrefab = hookPrefab;
                }
                GameObject characterInstance = Instantiate(characterPrefab, spawnPoints[playerID].position, Quaternion.identity);
                //테스트 코드. 추후 레전드 선택 기능 구현 시 코드 교체
                characterInstance.GetComponent<CharacterStatus>().InitCharacterType(Util.Enum.CharacterType.Peter);
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
<<<<<<< Updated upstream
        characterStatus.RespawnTime = _modeDefaultRespawnTime;
=======
        characterStatus.RepawnTime = _modeDefaultRespawnTime;
        characterStatus.OnRespawnSetting -= SetPlayerInputController;
        characterStatus.OnRespawnSetting += SetPlayerInputController;

>>>>>>> Stashed changes
        if (id > _teamSize)
        {
            characterStatus.TeamType = TeamType.Red;
            _teamMemberIndex = id - _teamSize - INDEX_OFFSET_FOR_ZERO;
            _teamRedCharacter[_teamMemberIndex] = characterStatus;
            character.layer = LayerMask.NameToLayer("TeamRed");
            characterStatus.TeamSpawnPoint = characterStatus.transform.position;
            character.name = "red";
        }
        else
        {
            characterStatus.TeamType = TeamType.Blue;
            _teamMemberIndex = id - INDEX_OFFSET_FOR_ZERO;
            _teamBlueCharacter[_teamMemberIndex] = characterStatus;
            character.layer = LayerMask.NameToLayer("TeamBlue");
            characterStatus.TeamSpawnPoint = characterStatus.transform.position;
            character.name = "blue";

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
                playercontroller.SwitchCurrentActionMap("FirstPlayerActions");

                break;
            case 2:
                playercontroller = character.GetComponent<UnityEngine.InputSystem.PlayerInput>();
                playercontroller.actions.name = "PlayerInput";
                playercontroller.SwitchCurrentControlScheme("PC");
                Keyboard keyBoard = InputSystem.GetDevice<Keyboard>();
                playercontroller.actions.devices = new InputDevice[] { keyBoard };
                playercontroller.SwitchCurrentActionMap("SecondPlayerActions");

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
    private IEnumerator UpdateGameTime()
    {
        while (false == _isGameOver && GameTime < _currentGameMode.MaxGameTime)
        {
            GameTime += Time.deltaTime;
            yield return null;
        }
        _isGameOver = true;
        EndGameMode();
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
            _isGameOver = true;
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
            winningTeam = CheckTeamHealthRatio();
            if (winningTeam == TeamType.None)
            {
                Debug.Log("무승부"); // 게임 종료 씬 구성 이후 무승부 시 연출 구현 필요
            }
            Debug.Log($"{winningTeam}팀 승리"); // 게임 종료 씬 구성 이후 승리 팀 연출 구현 필요
        }
        else
        {
            winningTeam = (teamBlueEndScore > teamRedEndScore) ? TeamType.Blue : TeamType.Red;
            Debug.Log($"게임 종료 {winningTeam}팀 승리"); // 게임 종료 씬 구성 이후 승리 팀 연출 구현 필요
        }
    }
    private TeamType CheckTeamHealthRatio()
    {
        int teamBlueCharacterHealthRatio = _teamBlueCharacter[0].HealthPointRatio;
        int teamRedCharacterHelathRatio = _teamRedCharacter[0].HealthPointRatio;

        if (teamBlueCharacterHealthRatio == teamRedCharacterHelathRatio)
            return TeamType.None;
        else if (teamBlueCharacterHealthRatio > teamRedCharacterHelathRatio)
            return TeamType.Blue;
        else
            return TeamType.Red;
    }
}

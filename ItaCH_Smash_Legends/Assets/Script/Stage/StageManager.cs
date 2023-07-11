using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using Util.Enum;

public class StageManager : MonoBehaviourPunCallbacks
{
    private const GameModeType DEFAULT_GAME_MODE = GameModeType.Duel;
    public GameMode CurrentGameMode { get => _currentGameMode; private set => _currentGameMode = value; }
    private GameMode _currentGameMode;
    private GameModeType _selectedGameMode;
    private CharacterStatus[] _playerCharacterInstances;
    public CharacterStatus[] Players { get => _playerCharacterInstances; }
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

    private void OnEnable()
    {
        Managers.LobbyManager.OnInGameSceneLoaded -= SetStage;
        Managers.LobbyManager.OnInGameSceneLoaded += SetStage;
    }

    public void Init()
    {
        GetGameMode(DEFAULT_GAME_MODE);
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
        _playerCharacterInstances = new CharacterStatus[_totalPlayer];
        _teamBlueCharacter = new CharacterStatus[_teamSize];
        _teamRedCharacter = new CharacterStatus[_teamSize];
        for (int playerID = 0; playerID < _totalPlayer; playerID++)
        {
            UserData userData = Managers.UserManager.GetUserData(playerID);
            CreateCharacter(userData, currentGameMode.SpawnPoints);
        }
        SetModeUI(currentGameMode.GameModeType);
        StartCoroutine(UpdateGameTime());
    }

    public void CreateMap(GameMode gameMode)
    {
        GameObject mapInstance = Instantiate(gameMode.Map, null);
    }

    public void CreateCharacter(UserData userData, Transform[] spawnPoints) // 캐릭터 선택 기능 구현 시 매개변수로 선택한 캐릭터 함께 전달
    {
        int playerID = userData.Id;
        CharacterType selectedCharacter = userData.SelectedCharacter;
        CharacterStatus characterPrefab = GetCharacterPrefab(selectedCharacter);

        if (characterPrefab != null)
        {
            if (spawnPoints[playerID] != null)
            {
                CharacterStatus characterInstance = Instantiate(characterPrefab, spawnPoints[playerID + INDEX_OFFSET_FOR_ZERO].position, Quaternion.identity);
                InitCharacterStatus(characterInstance, userData);
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
            Debug.LogError("Failed to load the prefab at path");
        }
    }

    private CharacterStatus GetCharacterPrefab(CharacterType character)
    {
        string characterName = character.ToString();
        string characterPrefabPath = Path.Combine(StringLiteral.PREFAB_FOLDER, characterName, $"{characterName}{StringLiteral.SUFFIX_INGAME}", $"{characterName}{StringLiteral.SUFFIX_INGAME}");
        return Managers.ResourceManager.Load<CharacterStatus>(characterPrefabPath);
    }
    private void InitCharacterStatus(CharacterStatus characterStatus, UserData userData)
    {
        int playerID = userData.Id;
        characterStatus.Init(userData);
        characterStatus.RespawnTime = _modeDefaultRespawnTime;
        SetTeam(characterStatus, playerID);

        characterStatus.OnRespawnSetting -= SetPlayerInputController;
        characterStatus.OnRespawnSetting += SetPlayerInputController;

        characterStatus.OnPlayerDie -= UpdateTeamScore;
        characterStatus.OnPlayerDie += UpdateTeamScore;
    }
    private void SetTeam(CharacterStatus character, int playerID) // TO DO : Team Class로 관리 필요
    {
        if (playerID >= _teamSize)
        {
            character.TeamType = TeamType.Red;
            _teamMemberIndex = playerID - _teamSize;
            _teamRedCharacter[_teamMemberIndex] = character;
            character.gameObject.layer = LayerMask.NameToLayer("TeamRed");
            character.SpawnPoint = character.transform.position;
            character.gameObject.name = "red";
        }

        else
        {
            character.TeamType = TeamType.Blue;
            _teamMemberIndex = playerID;
            _teamBlueCharacter[_teamMemberIndex] = character;
            character.gameObject.layer = LayerMask.NameToLayer("TeamBlue");
            character.SpawnPoint = character.transform.position;
            character.gameObject.name = "blue";
        }
    }
    public void SetPlayerInputController(CharacterStatus character, int id)
    {
        UnityEngine.InputSystem.PlayerInput playercontroller;

        switch (id)
        {
            case 0:
                playercontroller = character.GetComponent<UnityEngine.InputSystem.PlayerInput>();
                playercontroller.SwitchCurrentActionMap("FirstPlayerActions");
                break;

            case 1:
                playercontroller = character.GetComponent<UnityEngine.InputSystem.PlayerInput>();
                playercontroller.actions.name = "PlayerInput";
                playercontroller.SwitchCurrentActionMap("SecondPlayerActions");
                Keyboard keyBoard = InputSystem.GetDevice<Keyboard>();
                playercontroller.actions.devices = new InputDevice[] { keyBoard };
                break;

            default:
                return;
        }
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
                for (int i = 0; i < _totalPlayer; ++i)
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
    public void SetLegendUI(CharacterStatus player)
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
                Debug.Log("무승부"); // Result UI 스크립트와 연결 필요
            }
            Debug.Log($"{winningTeam}팀 승리"); // Result UI 스크립트와 연결 필요
        }
        else
        {
            winningTeam = (teamBlueEndScore > teamRedEndScore) ? TeamType.Blue : TeamType.Red;
            Debug.Log($"게임 종료 {winningTeam}팀 승리"); // Result UI 스크립트와 연결 필요
        }
    }
    private TeamType CheckTeamHealthRatio()
    {
        int teamBlueCharacterHealthRatio = _teamBlueCharacter[0].CurrentHPRatio;
        int teamRedCharacterHelathRatio = _teamRedCharacter[0].CurrentHPRatio;

        if (teamBlueCharacterHealthRatio == teamRedCharacterHelathRatio)
            return TeamType.None;
        else if (teamBlueCharacterHealthRatio > teamRedCharacterHelathRatio)
            return TeamType.Blue;
        else
            return TeamType.Red;
    }
}

using Cysharp.Threading.Tasks;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Util.Enum;

public class StageManager : MonoBehaviourPunCallbacks
{
    private const GameModeType DEFAULT_GAME_MODE = GameModeType.Duel;
    public GameMode CurrentGameMode
    {
        get
        {
            if (_currentGameMode == null)
            {
                SetGameMode(DEFAULT_GAME_MODE);
            }
            return _currentGameMode;
        }

        private set => _currentGameMode = value;
    }
    private GameMode _currentGameMode;

    private List<Team> _teams;

    private const int INDEX_OFFSET_FOR_ZERO = 1;

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
    private float _gameTime;
    private int _gameTimeInt;
    public int RemainGameTime => Mathf.Max(_currentGameMode.MaxGameTime - _gameTimeInt, 0);
    private bool _isGameOver;

    private GameObject _legendUIPrefab;
    private GameObject[] _modeUIPrefab;

    private List<GameObject> _legendUI;
    private GameObject _modeUI;

    public event Action<int> OnTimeChange;

    public override void OnEnable()
    {
        Managers.LobbyManager.OnInGameSceneLoaded -= SetStage;
        Managers.LobbyManager.OnInGameSceneLoaded += SetStage;
    }

    public void Init()
    {
        SetGameMode(DEFAULT_GAME_MODE);
    }

    public void SetGameMode(GameModeType selectedMode)
    {
        if (_currentGameMode == null)
        {
            _currentGameMode = new GameMode();
        }
        _currentGameMode.Init(selectedMode);
        _teams = new List<Team>();
        _isGameOver = false;
    }

    public void ChangeGameMode(GameModeType selectedMode) // 게임 모드 선택 기능 구현 후 사용
    {
        if (_currentGameMode.GameModeType == selectedMode)
        {
            return;
        }
        else
        {
            SetGameMode(selectedMode);
        }
    }

    public void SetStage(GameMode currentGameMode)
    {
        InstantiateMap();
        for (int userID = 0; userID < _currentGameMode.MaxPlayer; ++userID)
        {
            UserData userData = Managers.UserManager.GetUserData(userID);
            SetUserTeam(userData);
            CreateCharacter(userData, currentGameMode.SpawnPoints);
        }
        SetModeUI(currentGameMode.GameModeType);
        StartGame();
    }

    private void InstantiateMap() => Managers.ResourceManager.Instantiate(_currentGameMode.Map);

    private void SetUserTeam(UserData user)
    {
        Team team = GetTeamAvailable();
        team.AddMember(user);
    }
    private Team GetTeamAvailable()
    {
        int last = _teams.Count - INDEX_OFFSET_FOR_ZERO;

        if (_teams.Count == 0 || _teams[last].MemberCount == CurrentGameMode.MaxTeamMember)
        {
            Team newTeam = new Team();
            _teams.Add(newTeam);
            newTeam.TeamColor = (_teams.Count == 0) ? TeamType.Blue : TeamType.Red;
            return newTeam;
        }
        else
        {
            return _teams[last];
        }
    }

    public void CreateCharacter(UserData user, Transform[] spawnPoints)
    {
        LegendController characterPrefab = GetCharacterPrefab(user.SelectedCharacter);

        if (characterPrefab != null)
        {
            if (spawnPoints[user.ID] != null)
            {
                GameObject characterInstance = Managers.ResourceManager.Instantiate(characterPrefab.gameObject, spawnPoints[user.ID + INDEX_OFFSET_FOR_ZERO]); // spawnPoint[0] == root gameObject
                LegendController legend = characterInstance.GetComponent<LegendController>();
                legend.Init(user.SelectedCharacter, user.ID);
                legend.gameObject.layer =
                    (user.Team == TeamType.Blue) ?
                    LayerMask.NameToLayer(StringLiteral.TEAM_BLUE) : LayerMask.NameToLayer(StringLiteral.TEAM_RED);
            }
            else
            {
                Debug.LogError(user.ID + "P character spawn position is Null");
            }
        }
        else
        {
            Debug.LogError("Failed to load the prefab at path");
        }
    }

    private LegendController GetCharacterPrefab(CharacterType character)
    {
        string characterName = character.ToString();
        string characterPrefabPath = Path.Combine(StringLiteral.PREFAB_FOLDER, characterName, $"{characterName}{StringLiteral.SUFFIX_INGAME}", $"{characterName}{StringLiteral.SUFFIX_INGAME}");
        return Managers.ResourceManager.Load<LegendController>(characterPrefabPath);
    }

    public void SetModeUI(GameModeType gameModeType) // TODO : UI에서 하도록 수정 필요
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
                for (int i = 0; i < _currentGameMode.MaxPlayer; ++i)
                {
                    //SetLegendUI(_playerCharacterInstances[i]); // TO DO : 레전드 UI 리팩토링
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
    public void SetLegendUI(LegendController player) // TO DO : UI가 직접 하도록 수정 필요
    {
        _legendUIPrefab = Resources.Load<GameObject>("UI/LegendUI");
        GameObject legendUI = Instantiate(_legendUIPrefab);
        //legendUI.GetComponent<LegendUI>().InitLegendUISettings(player.transform); // TO DO : CharacterStatus가 아닌 곳에서 Stat 가져오고 참조 연결
        _legendUI.Add(legendUI);
    }

    public void StartGame()
    {
        // TO DO : 게임모드 소개 패널 연출 >> Smash!! 패널 연출 >> 체력 차오르는 연출 >> 게임 돌입        
        // 전부 캐릭터 생성 이후 대기 애니메이션 재생 동안 실행
        UpdateGameTime();
    }
    private async UniTask UpdateGameTime() // StartGame에서 호출
    {
        while (false == _isGameOver && GameTime < _currentGameMode.MaxGameTime)
        {
            GameTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }
        _isGameOver = true;
        EndGame();
    }
    // TO DO : 승점 판정 로직 팀 로직 변경 이후 관리 필요
    private void EndGame()
    {
        //    int teamBlueEndScore = GetTeamScore(TeamType.Blue);
        //    int teamRedEndScore = GetTeamScore(TeamType.Red);
        //    TeamType winningTeam = TeamType.None;

        //    if (teamBlueEndScore == teamRedEndScore)
        //    {
        //        winningTeam = CheckTeamHealthRatio();
        //        if (winningTeam == TeamType.None)
        //        {
        //            Debug.Log("무승부"); // Result UI 스크립트와 연결 필요
        //        }
        //        Debug.Log($"{winningTeam}팀 승리"); // Result UI 스크립트와 연결 필요
        //    }
        //    else
        //    {
        //        winningTeam = (teamBlueEndScore > teamRedEndScore) ? TeamType.Blue : TeamType.Red;
        //        Debug.Log($"게임 종료 {winningTeam}팀 승리"); // Result UI 스크립트와 연결 필요
        //    }
    }
}

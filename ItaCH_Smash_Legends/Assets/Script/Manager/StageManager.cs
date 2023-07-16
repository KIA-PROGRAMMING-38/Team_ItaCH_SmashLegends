using Cysharp.Threading.Tasks;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

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
                OnTimeChange?.Invoke(RemainGameTime);
            }
        }
    }
    private float _gameTime;
    private int _gameTimeInt; // TO DO : 위와 중복 해당 부분 정리 필요
    public int RemainGameTime => Mathf.Max(_currentGameMode.MaxGameTime - _gameTimeInt, 0);
    public bool IsTimeOver { get => _isTimeOver; }
    private bool _isTimeOver;

    public bool IsGameOver { get => _isGameOver; }
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
        _isTimeOver = false;
        _isGameOver = false;
        InstantiateMap();
        for (int i = 0; i < _currentGameMode.MaxPlayer; ++i)
        {
            UserData userData = Managers.UserManager.GetUserData(i);
            SetUserTeam(userData);
            CreateLegend(userData, currentGameMode.SpawnPoints[i + 1]); // SpawnPoints[0] == root Object
        }
        SetModeUI(currentGameMode.GameModeType);
        StartGame();
    }

    private void InstantiateMap() => Managers.ResourceManager.Instantiate(_currentGameMode.Map);

    private void SetUserTeam(UserData user)
    {
        Team team = GetTeamAvailable();
        team.AddMember(user);

        Team GetTeamAvailable()
        {
            if (IsNewTeamNeeded(_teams))
            {
                Team newTeam = new Team();
                _teams.Add(newTeam);
                newTeam.Type = (_teams.Count == 1) ? TeamType.Blue : TeamType.Red;
                return newTeam;
            }
            else
            {
                return _teams[^1];
            }

            bool IsNewTeamNeeded(List<Team> teams) => teams.Count == 0 || teams[^1].Members.Count == _currentGameMode.MaxTeamMember;
        }
    }

    public void CreateLegend(UserData user, Transform spawnPoint)
    {
        LegendController legend = Managers.ResourceManager.GetLegendPrefab(user.SelectedLegend);
        GameObject legendObject = Managers.ResourceManager.Instantiate(legend.gameObject, spawnPoint);

        legend.Init(user);

        legendObject.layer =
            (user.Team.Type == TeamType.Blue) ?
            LayerMask.NameToLayer(StringLiteral.TEAM_BLUE) : LayerMask.NameToLayer(StringLiteral.TEAM_RED);
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
        UpdateGameTimeAsync();
    }

    private async UniTask UpdateGameTimeAsync()
    {
        while (false == _isGameOver && GameTime < _currentGameMode.MaxGameTime)
        {
            GameTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }
        _isTimeOver = true;
        EndGame(TeamType.None);
    }

    public void EndGame(TeamType winningTeam)
    {
        _isGameOver = true;
        if (TeamType.None == winningTeam)
        {
            winningTeam = _currentGameMode.GetWinningTeam(in _teams);
        }

        // 이벤트로 뿌리자
    }
}

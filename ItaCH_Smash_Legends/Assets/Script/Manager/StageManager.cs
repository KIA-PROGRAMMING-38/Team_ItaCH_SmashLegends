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
                _currentGameMode = new GameMode();
                _currentGameMode.Init(DEFAULT_GAME_MODE);
            }
            return _currentGameMode;
        }

        private set => _currentGameMode = value;
    }
    private GameMode _currentGameMode;

    private Transform[] _spawnPoints;

    public float GameTime
    {
        get => Mathf.FloorToInt(_gameTime);
        set
        {
            _gameTime = value;
            OnTimeChange?.Invoke(RemainGameTime);
        }
    }
    private float _gameTime;
    public int RemainGameTime => Mathf.Max(_currentGameMode.MaxGameTime - (int)GameTime, 0);
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
        Managers.LobbyManager.OnInGameSceneLoaded -= () => SetUpStage(_currentGameMode);
        Managers.LobbyManager.OnInGameSceneLoaded += () => SetUpStage(_currentGameMode);
    }

    public void Init()
    {

    }

    public void ChangeGameMode(GameModeType selectedMode) // 게임 모드 선택 기능 구현 후 사용
    {
        if (_currentGameMode.GameModeType == selectedMode)
        {
            return;
        }
        else
        {
            _currentGameMode.Init(selectedMode);
        }
    }

    public void SetUpStage(GameMode currentGameMode)
    {
        _isTimeOver = false;
        _isGameOver = false;
        InstantiateMap(currentGameMode.Map);
        foreach (Team team in currentGameMode.Teams)
        {
            foreach (UserData member in team.Members)
            {
                CreateLegend(member, _spawnPoints[member.ID + 1]); // SpawnPoints[0] == root Object
            }
        }
        Managers.UIManager.ShowPopupUI<UI_DuelModePopup>();
        StartGame();
    }

    private void InstantiateMap(string mapPath)
    {
        GameObject mapObject = Managers.ResourceManager.Instantiate(mapPath);
        _spawnPoints = mapObject.transform.Find(StringLiteral.SPAWN_POINTS).GetComponentsInChildren<Transform>();
    }

    public void CreateLegend(UserData user, Transform spawnPoint)
    {
        GameObject legendObject = Managers.ResourceManager.Instantiate
            (Managers.ResourceManager.GetLegendPrefab(user.SelectedLegend), null);

        LegendController legendController = legendObject.GetComponent<LegendController>();
        legendController.Init(user);

        legendObject.transform.position = spawnPoint.position;
        legendObject.layer =
            (user.TeamType == TeamType.Blue) ?
            LayerMask.NameToLayer(StringLiteral.TEAM_BLUE) : LayerMask.NameToLayer(StringLiteral.TEAM_RED);
    }

    public void SetModeUI(GameModeType gameModeType) // TO DO : UI에서 하도록 수정 필요
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
        // TO DO : 
        // 1) 게임모드 : 모드 UI 연출 + 모드 소개 패널 연출 >> 차오르는 연출 1
        // 2) 이때 부터 모드 0부터 남은 시간까지 타이머 역순으로 올라감
        // 3) 레전드 전부 모델 생성 및 UI 함꼐 출현
        // 4) 모드 UI 초상화 연출
        // 5) 팔로우캠 타겟 맵 전체 >> 자신의 캐릭터
        // 6) Smash!! 패널 연출 >> 체력 차오르는 연출 >> 게임 돌입        
        UpdateGameTimeAsync();
        // 전부 캐릭터 생성 이후 대기 애니메이션 재생 동안 실행
    }

    private async UniTask UpdateGameTimeAsync()
    {
        while (false == _isGameOver && GameTime < _currentGameMode.MaxGameTime)
        {
            GameTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }
        _isTimeOver = true;
        _currentGameMode.IsOver();
    }

    public void EndGame(Team winnerTeam) // gameMode가 승점 계산 이후 Invoke
    {
        // 게임 종료 연출 실행
        // >> 승리 팀 색의 Match Over 패널 Pop

        // >> 결과 패널 Pop
        // 로비 씬 전환
        //  Result UI >> 로컬 유저 팀 멤버 로비 모델 가져와 애니메이션 재생 및 승패 여부
    }
}

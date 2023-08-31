using Cysharp.Threading.Tasks;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public int RemainGameTime
    {
        get => (int)Mathf.Max(_remainGameTime, 0);
        set
        {
            _remainGameTime = value;
        }
    }
    private float _remainGameTime;
    public bool IsTimeOver { get => _isTimeOver; }
    private bool _isTimeOver;

    public bool IsGameOver { get => _isGameOver; }
    private bool _isGameOver;

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

        Managers.UIManager.ShowPopupUI<UI_DuelModePopup>();

        foreach (Team team in currentGameMode.Teams)
        {
            if (team.Type == TeamType.None)
            {
                continue;
            }

            foreach (UserData member in team.Members)
            {
                CreateLegend(member, _spawnPoints[(int)member.TeamType]); // SpawnPoints[0] == root Object
            }
        }

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

    public void StartGame()
    {
        _remainGameTime = _currentGameMode.MaxGameTime;
        // TO DO : 
        // 1) 게임모드 : 모드 UI 연출 + 모드 소개 패널 연출 >> 차오르는 연출 1
        // 2) 이때 부터 모드 0부터 남은 시간까지 타이머 역순으로 올라감
        // 3) 레전드 전부 모델 생성 및 UI 함꼐 출현
        // 4) 모드 UI 초상화 연출
        // 5) 팔로우캠 타겟 맵 전체 >> 자신의 캐릭터
        // 6) Smash!! 패널 연출 >> 체력 차오르는 연출 >> 게임 돌입        
        UpdateRemainGameTimeAsync().Forget();
        // 전부 캐릭터 생성 이후 대기 애니메이션 재생 동안 실행
    }

    private async UniTask UpdateRemainGameTimeAsync()
    {
        while (false == _isGameOver && RemainGameTime > 0)
        {
            _remainGameTime -= Time.deltaTime;
            OnTimeChange?.Invoke(RemainGameTime);

            await UniTask.Yield();
        }
        _isTimeOver = true;
        _currentGameMode.IsOver();
    }

    public void EndGame(TeamType winnerTeam)
    {
        // 게임 종료 연출 실행
        // >> 승리 팀 색의 Match Over 패널 Pop        
        SceneManager.LoadScene(StringLiteral.RESULT);
        Managers.UIManager.ClosePopupUI();
        UI_GameResultPopup popup = Managers.UIManager.ShowPopupUI<UI_GameResultPopup>();
        popup.SetInfo(winnerTeam);

        //  Result UI >> 로컬 유저 팀 멤버 로비 모델 가져와 애니메이션 재생 및 승패 여부
    }
}

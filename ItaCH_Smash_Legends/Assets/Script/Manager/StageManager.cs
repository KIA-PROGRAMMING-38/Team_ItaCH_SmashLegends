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

        foreach (Team team in currentGameMode.Teams)
        {
            if (team.Type == TeamType.None)
            {
                continue;
            }

            foreach (UserData member in team.Members)
            {
                CreateLegend(member, _spawnPoints[(int)member.TeamType]);
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
        legendController.Init(user, spawnPoint);

        legendObject.layer =
            (user.TeamType == TeamType.Blue) ?
            LayerMask.NameToLayer(StringLiteral.TEAM_BLUE) : LayerMask.NameToLayer(StringLiteral.TEAM_RED);
    }

    public void StartGame()
    {
        _remainGameTime = _currentGameMode.MaxGameTime;
        // TO DO : 
        // 1) 게임모드 : 모드 UI 연출 + 모드 소개 패널 연출 >> 차오르는 연출 1
        Managers.UIManager.FindPopup<UI_DuelModePopup>().RefreshPopupUI();

        Managers.SoundManager.Play(SoundType.BGM, StringLiteral.BGM_STAGE);
        Managers.SoundManager.Play(SoundType.SFX, StringLiteral.SFX_MATCH_START);
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
        if (_isGameOver)
        {
            return;
        }

        while (false == _isGameOver && RemainGameTime > 0)
        {
            _remainGameTime -= Time.deltaTime;
            OnTimeChange?.Invoke(RemainGameTime);

            await UniTask.Yield();
        }

        if (RemainGameTime <= 0)
        {
            _isTimeOver = true;
            _currentGameMode.IsOver();
            return;
        }
    }

    public void EndGame(TeamType winnerTeam)
    {
        _isGameOver = true;
        // To Do : 게임 종료 연출 실행, 현재 부자연스럽고 급작스럽게 씬 전환 발생
        // >> 승리 팀 색의 Match Over 패널 Pop                
        SceneManager.LoadScene(StringLiteral.RESULT);
        Managers.UIManager.ClosePopupUI();
        UI_GameResultPopup popup = Managers.UIManager.ShowPopupUI<UI_GameResultPopup>();
        popup.SetInfo(winnerTeam);

        _isGameOver = false;
        _isTimeOver = false;
    }
}

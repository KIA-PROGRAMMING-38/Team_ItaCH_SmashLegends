using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public StageManager StageManager { get; private set; }
    public LobbyManager LobbyManager { get; private set; }
    public UserManager UserManager { get; private set; }

    public DataTable CharacterTable { get => _characterTable; private set => _characterTable = value; }
    private DataTable _characterTable;
    public LobbyUI LobbyUI;
    private TextMeshProUGUI _connectionInfoText;
    private Canvas _logInCanvas;
    public event Action OnStartGame;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        _logInCanvas = GetComponentInChildren<Canvas>();
        _connectionInfoText = transform.GetChild(0).GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        CreateMangerObjects();
        _characterTable = new DataTable();
        _characterTable.SetDataTable();
    }

    public void StartGame()
    // 현재 인스펙터 창에서 직접 할당 및 로그인 패널 버튼 입력 시 실행
    {
        OnStartGame.Invoke();
    }

    private void CreateMangerObjects()
    {
        GameObject newManagerObject;

        // 로비 매니저 오브젝트 생성
        newManagerObject = new GameObject(nameof(LobbyManager));
        newManagerObject.transform.parent = transform;
        LobbyManager = newManagerObject.AddComponent<LobbyManager>();
        LobbyManager.ConnectionInfoText = _connectionInfoText;
        LobbyManager.OnLogInSuccess -= LogInSuccess;
        LobbyManager.OnLogInSuccess += LogInSuccess;
        LobbyManager.OnMatchSuccess -= MatchSuccess;
        LobbyManager.OnMatchSuccess += MatchSuccess;

        // 유저 매니저 오브젝트 생성
        newManagerObject = new GameObject(nameof(UserManager));
        newManagerObject.transform.parent = transform;
        UserManager = newManagerObject.AddComponent<UserManager>();
        LobbyUI.OnCharacterChanged -= UserManager.UserLocalData.SetSelectedCharacter;
        LobbyUI.OnCharacterChanged += UserManager.UserLocalData.SetSelectedCharacter;

        // 스테이지 매니저 오브젝트 생성
        newManagerObject = new GameObject(nameof(StageManager));
        newManagerObject.transform.parent = transform;
        StageManager = newManagerObject.AddComponent<StageManager>();
        newManagerObject.SetActive(false);
    }

    public void LogInSuccess()
    // Photon의 OnConnectedToMaster() 서버 접속 성공 콜백 실행 시 실행
    {
        _logInCanvas.gameObject.SetActive(false);
        LobbyUI.transform.parent.gameObject.SetActive(true);
    }
    public void MatchSuccess(GameModeType gameModeType)
    {
        StageManager.gameObject.SetActive(true);
    }
}

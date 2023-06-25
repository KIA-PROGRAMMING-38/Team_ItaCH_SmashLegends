using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public StageManager StageManager { get; private set; }
    public LobbyManager LobbyManager { get; private set; }
    public UserManager UserManager { get; private set; }

    public GameObject LobbyUI;
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
    }

    public void StartGame()
    // 현재 인스펙터 창에서 직접 할당 및 로그인 패널 버튼 입력 시 실행
    {
        OnStartGame.Invoke();
    }

    private void CreateMangerObjects()
    {
        GameObject gameObject;

        // 로비 매니저 오브젝트 생성
        gameObject = new GameObject(nameof(LobbyManager));
        gameObject.transform.parent = transform;
        LobbyManager = gameObject.AddComponent<LobbyManager>();
        LobbyManager.ConnectionInfoText = _connectionInfoText;
        LobbyManager.OnLogInSuccess -= OnLogInSuccess;
        LobbyManager.OnLogInSuccess += OnLogInSuccess;
        LobbyManager.OnMatchSuccess -= OnMatchSuccess;
        LobbyManager.OnMatchSuccess += OnMatchSuccess;

        gameObject = new GameObject(nameof(UserManager));
        gameObject.transform.parent = transform;
        UserManager = gameObject.AddComponent<UserManager>();

        // 스테이지 매니저 오브젝트 생성
        gameObject = new GameObject(nameof(StageManager));
        gameObject.transform.parent = transform;
        StageManager = gameObject.AddComponent<StageManager>();
        gameObject.SetActive(false);
    }

    public void OnLogInSuccess()
    // Photon의 OnConnectedToMaster() 서버 접속 성공 콜백 실행 시 실행
    {
        _logInCanvas.gameObject.SetActive(false);
        LobbyUI.SetActive(true);
    }
    public void OnMatchSuccess(GameModeType gameModeType)
    {
        StageManager.gameObject.SetActive(true);
    }
}

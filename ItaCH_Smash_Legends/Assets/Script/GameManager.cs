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
    // ���� �ν����� â���� ���� �Ҵ� �� �α��� �г� ��ư �Է� �� ����
    {
        OnStartGame.Invoke();
    }

    private void CreateMangerObjects()
    {
        GameObject gameObject;

        // �κ� �Ŵ��� ������Ʈ ����
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

        // �������� �Ŵ��� ������Ʈ ����
        gameObject = new GameObject(nameof(StageManager));
        gameObject.transform.parent = transform;
        StageManager = gameObject.AddComponent<StageManager>();
        gameObject.SetActive(false);
    }

    public void OnLogInSuccess()
    // Photon�� OnConnectedToMaster() ���� ���� ���� �ݹ� ���� �� ����
    {
        _logInCanvas.gameObject.SetActive(false);
        LobbyUI.SetActive(true);
    }
    public void OnMatchSuccess(GameModeType gameModeType)
    {
        StageManager.gameObject.SetActive(true);
    }
}

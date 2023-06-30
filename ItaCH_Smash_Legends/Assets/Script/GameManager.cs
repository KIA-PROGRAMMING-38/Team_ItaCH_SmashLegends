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
    // ���� �ν����� â���� ���� �Ҵ� �� �α��� �г� ��ư �Է� �� ����
    {
        OnStartGame.Invoke();
    }

    private void CreateMangerObjects()
    {
        GameObject newManagerObject;

        // �κ� �Ŵ��� ������Ʈ ����
        newManagerObject = new GameObject(nameof(LobbyManager));
        newManagerObject.transform.parent = transform;
        LobbyManager = newManagerObject.AddComponent<LobbyManager>();
        LobbyManager.ConnectionInfoText = _connectionInfoText;
        LobbyManager.OnLogInSuccess -= LogInSuccess;
        LobbyManager.OnLogInSuccess += LogInSuccess;
        LobbyManager.OnMatchSuccess -= MatchSuccess;
        LobbyManager.OnMatchSuccess += MatchSuccess;

        // ���� �Ŵ��� ������Ʈ ����
        newManagerObject = new GameObject(nameof(UserManager));
        newManagerObject.transform.parent = transform;
        UserManager = newManagerObject.AddComponent<UserManager>();
        LobbyUI.OnCharacterChanged -= UserManager.UserLocalData.SetSelectedCharacter;
        LobbyUI.OnCharacterChanged += UserManager.UserLocalData.SetSelectedCharacter;

        // �������� �Ŵ��� ������Ʈ ����
        newManagerObject = new GameObject(nameof(StageManager));
        newManagerObject.transform.parent = transform;
        StageManager = newManagerObject.AddComponent<StageManager>();
        newManagerObject.SetActive(false);
    }

    public void LogInSuccess()
    // Photon�� OnConnectedToMaster() ���� ���� ���� �ݹ� ���� �� ����
    {
        _logInCanvas.gameObject.SetActive(false);
        LobbyUI.transform.parent.gameObject.SetActive(true);
    }
    public void MatchSuccess(GameModeType gameModeType)
    {
        StageManager.gameObject.SetActive(true);
    }
}

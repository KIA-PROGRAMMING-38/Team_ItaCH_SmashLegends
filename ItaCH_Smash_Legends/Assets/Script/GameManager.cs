using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public StageManager StageManager { get; private set; }
    public LobbyManager LobbyManager { get; private set; }
    public Button PlayGameButton { get; set; }
    private TextMeshProUGUI _connectionInfoText;
    private Canvas _logInCanvas;
    public event Action StartGame;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _connectionInfoText = GetComponentInChildren<TextMeshProUGUI>();
        _logInCanvas = GetComponentInChildren<Canvas>();
        Instance = this;        
        CreateMangerObjects();
    }
    public void OnStartGame()
    {
        StartGame.Invoke();
    }
    public void OnLogInSuccess()
    {
        _logInCanvas.gameObject.SetActive(false);
    }

    private void CreateMangerObjects()
    {
        GameObject gameObject;
        gameObject = new GameObject(nameof(LobbyManager));
        gameObject.transform.parent = transform;
        LobbyManager = gameObject.AddComponent<LobbyManager>();
        LobbyManager.ConnectionInfoText = _connectionInfoText;        

        gameObject = new GameObject(nameof(StageManager));
        gameObject.transform.parent = transform;
        StageManager = gameObject.AddComponent<StageManager>();
        gameObject.SetActive(false);
    }
}

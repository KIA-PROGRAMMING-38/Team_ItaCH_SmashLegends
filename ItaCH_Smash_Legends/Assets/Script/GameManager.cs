using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameManager Instance;
    public StageManager StageManager { get; private set; }
    public LobbyManager LobbyManager { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        CreateMangerObjects();
        Instance = this;
    }
    private void CreateMangerObjects()
    {
        GameObject gameObject;
        gameObject = new GameObject(nameof(StageManager));
        gameObject.transform.parent = transform;
        StageManager = gameObject.AddComponent<StageManager>();

        gameObject = new GameObject(nameof(LobbyManager));
        gameObject.transform.parent = transform;
        LobbyManager = gameObject.AddComponent<LobbyManager>();
    }
}

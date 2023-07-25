using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers s_instance = null;
    public static Managers Instance { get { Init(); return s_instance; } }

    private static StageManager s_stageManager;
    private static LobbyManager s_lobbyManager;
    private static DataManager s_dataManager = new DataManager();
    private static ResourceManager s_resourceManager = new ResourceManager();
    private static UIManager s_uiManager = new UIManager();

    public static StageManager StageManager { get { Init(); return s_stageManager; } }
    public static LobbyManager LobbyManager { get { Init(); return s_lobbyManager; } }
    public static UIManager UIManager { get { Init(); return s_uiManager; } }
    public static DataManager DataManager { get { Init(); return s_dataManager; } }
    public static ResourceManager ResourceManager { get { Init(); return s_resourceManager; } }

    private void Start()
    {
        Init();
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject gameObject = GameObject.Find("@Managers");
            if (gameObject == null)
            {
                gameObject = new GameObject { name = "@Managers" };
                s_instance = gameObject.AddComponent<Managers>();

                DontDestroyOnLoad(gameObject);

                s_lobbyManager = CreateManager<LobbyManager>();
                s_stageManager = CreateManager<StageManager>();

                s_lobbyManager.Init();
                s_stageManager.Init();
                s_dataManager.Init();
                s_resourceManager.Init();
                s_uiManager.Init();
            }
        }
    }

    private static T CreateManager<T>() where T : UnityEngine.Component
    {
        GameObject go = new GameObject($"@{typeof(T)}");
        T result = go.AddComponent<T>();
        go.transform.SetParent(s_instance.transform);

        return result;
    }
}
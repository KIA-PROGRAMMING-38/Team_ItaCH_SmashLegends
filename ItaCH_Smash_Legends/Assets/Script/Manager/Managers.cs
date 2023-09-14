using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers s_instance = null;
    public static Managers Instance { get { Init(); return s_instance; } }

    private static StageManager s_stageManager;
    private static LobbyManager s_lobbyManager;
    private static DataManager s_dataManager;
    private static ResourceManager s_resourceManager;
    private static SoundManager s_soundManager;
    private static UIManager s_uiManager;

    public static StageManager StageManager { get { Init(); return s_stageManager; } }
    public static LobbyManager LobbyManager { get { Init(); return s_lobbyManager; } }
    public static UIManager UIManager { get { Init(); return s_uiManager; } }
    public static DataManager DataManager { get { Init(); return s_dataManager; } }
    public static ResourceManager ResourceManager { get { Init(); return s_resourceManager; } }
    public static SoundManager SoundManager { get { Init(); return s_soundManager; } }

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
            }

            s_instance = Utils.GetOrAddComponent<Managers>(gameObject);
            DontDestroyOnLoad(gameObject);


            s_lobbyManager = CreateManager<LobbyManager>();
            s_stageManager = CreateManager<StageManager>();
            s_soundManager = CreateManager<SoundManager>();
            s_uiManager = CreateManager<UIManager>();
            s_dataManager = new DataManager();
            s_resourceManager = new ResourceManager();

            s_lobbyManager.Init();
            s_stageManager.Init();
            s_soundManager.Init();
            s_uiManager.Init();
            s_dataManager.Init();
            s_resourceManager.Init();
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
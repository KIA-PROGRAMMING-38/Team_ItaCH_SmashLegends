using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers s_instance = null;
    public static Managers Instance { get { Init(); return s_instance; } }

    private static StageManager s_stageManager;
    private static LobbyManager s_lobbyManager;
    private static UserManager s_userManager = new UserManager();

    public static StageManager StageManager { get { Init(); return s_stageManager; } }
    public static LobbyManager LobbyManager { get { Init(); return s_lobbyManager; } }
    public static UserManager UserManager { get { Init(); return s_userManager; } }

    // DataManager 구성 이후 옮겨갈 부분
    public DataTable CharacterTable { get => _characterTable; private set => _characterTable = value; }
    private DataTable _characterTable;

    private void Start()
    {
        Init();
        // DataManager 구성 이후 옮겨갈 부분
        _characterTable = new DataTable();
        _characterTable.SetDataTable();
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject gameObject = GameObject.Find("@Managers");
            if (gameObject == null)
            {
                gameObject = new GameObject { name = "@managers" };
                s_instance = gameObject.AddComponent<Managers>();

                DontDestroyOnLoad(gameObject);

                s_lobbyManager = CreateManager<LobbyManager>();
                s_stageManager = CreateManager<StageManager>();

                s_userManager.Init();
                s_lobbyManager.Init();
                s_stageManager.Init();
            }
        }
    }

    private static T CreateManager<T>() where T : UnityEngine.Component
    {
        var go = new GameObject($"@{typeof(T)}");
        T result = go.AddComponent<T>();
        go.transform.SetParent(s_instance.transform);

        return result;
    }
}
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers s_instance = null;
    public static Managers Instance { get { return s_instance; } }

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
                gameObject = new GameObject { name = "@Managers" };
                s_instance = gameObject.AddComponent<Managers>();
                DontDestroyOnLoad(gameObject);

                gameObject = new GameObject { name = "@LobbyManager" };
                s_lobbyManager = gameObject.AddComponent<LobbyManager>();
                gameObject.transform.SetParent(s_instance.transform);

                gameObject = new GameObject { name = "@StageManager" };
                s_stageManager = gameObject.AddComponent<StageManager>();
                gameObject.transform.SetParent(s_instance.transform);

                s_userManager.Init();
            }
            s_instance = gameObject.GetComponent<Managers>();
        }
    }    
}
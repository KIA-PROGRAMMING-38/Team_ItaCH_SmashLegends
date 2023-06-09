using UnityEngine;

public class StageManager : MonoBehaviour
{
    private const GameModeType DEFAULT_GAME_MODE = GameModeType.Duel;
    private GameMode _currentGameMode;
    private GameModeType _selectedGameMode;
    private GameObject[] _playerCharacterInstances;
    private int _totalPlayer;

    // 테스트를 위해 인스펙터 창을 사용. 추후 리소스 폴더에서 로드하는 것으로 변경;
    [SerializeField] private GameObject _legendUIPrefab;
    [SerializeField] private GameObject[] _modeUIPrefab;
    private List<GameObject> _legendUI;
    private GameObject _modeUI;
    private void Awake()
    {
        GetGameMode(DEFAULT_GAME_MODE);
    }
    private void Start()
    {
        SetStage(_currentGameMode);
    }
    public void GetGameMode(GameModeType gameModeSelected)
    {
        if (_currentGameMode == null)
        {
            _currentGameMode = new GameMode();
        }
        _currentGameMode.InitGameMode(gameModeSelected);
        _totalPlayer = _currentGameMode.TotalPlayer;
    }
    public void SetStage(GameMode currentGameMode)
    {
        CreateMap(currentGameMode);
        _playerCharacterInstances = new GameObject[_totalPlayer + 1]; // index와 playerID를 일치시키기 위한 +1

        for (int playerID = 1; playerID <= _totalPlayer; playerID++)
        {
            _1PCharacter = Instantiate(_characterPrefab, _1pSpawnPoint);
            _2PCharacter = Instantiate(_characterPrefab, _2pSpawnPoint);
        }
        else
        {
            Debug.LogError("Failed to load the prefab at path: " + _characterPrefabPath);
        }
        // InGame Scene 불러와야 함.
        // _currentGameMode.Map의 게임모드 타입과 연결되는 맵 프리펩 불러와 Instanciate
        SetModeUI(_selectedGameMode);
            CreateCharacter(playerID, currentGameMode.SpawnPoints);
        }
    }
    public void CreateCharacter(int playerID, Transform[] spawnPoints) // 캐릭터 선택 기능 구현 시 매개변수로 선택한 캐릭터 함께 전달
    {
        string characterPrefabPath = "Charater/Peter/Peter_Ingame/Peter_Ingame"; // 캐릭터 선택 기능 구현 시 캐릭터 이름으로 경로 구성 필요
        GameObject characterPrefab = Resources.Load<GameObject>(characterPrefabPath);

        if (characterPrefab != null)
        {
            if (spawnPoints[playerID] != null)
            {
                GameObject characterInstance = Instantiate(characterPrefab, spawnPoints[playerID].position, Quaternion.identity);
                _playerCharacterInstances[playerID] = characterInstance;
            }
            else Debug.LogError(playerID + "P character spawn position is Null");
        }
        else
        {
            Debug.LogError("Failed to load the prefab at path: " + characterPrefabPath);
        }
    }
    public void CreateMap(GameMode gameMode)
    {
        GameObject mapInstance = Instantiate(gameMode.Map);
    }
    public void ChangeGameMode(GameModeType gameModeSelected)
    {
        if (_selectedGameMode == gameModeSelected)
        {
            return;
        }
        else
        {
            GetGameMode(gameModeSelected);
        }
    }

    public void SetModeUI(GameModeType gameModeType)
    {
        switch (gameModeType)
        {
            case GameModeType.None:
                Debug.Log("Failed to Find ModeUI" + $"{gameModeType}");
                break;
            case GameModeType.Duel:
                _legendUI = new List<GameObject>();
                SetLegendUI(_1PCharacter);
                SetLegendUI(_2PCharacter);
                _modeUI = Instantiate(_modeUIPrefab[(int)GameModeType.Duel]);
                //추후 스테이지에 존재하는 레전드를 하나로 관리하는 배열 생성하여 foreach로 생성.
                break;
            case GameModeType.TeamMatch:
                // 듀얼과 유사한 로직으로 구현
                Debug.Log("Failed to Find ModeUI" + $"{gameModeType}");
                break;
        }
    }
    public void SetLegendUI(GameObject player)
    {
        GameObject legendUI = Instantiate(_legendUIPrefab);
        legendUI.GetComponent<LegendUI>().InitLegendUISettings(player.transform);
        _legendUI.Add(legendUI);
    }
}

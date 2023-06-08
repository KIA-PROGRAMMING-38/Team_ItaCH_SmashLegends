using UnityEngine;

public class StageManager : MonoBehaviour
{
    private const GameModeType DEFAULT_GAME_MODE = GameModeType.Duel;
    private GameMode _currentGameMode;
    private GameModeType _selectedGameMode;
    private GameObject[] _playerCharacterInstances;
    private int _totalPlayer;

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
}

using TMPro;
using UnityEngine;
using Util.Enum;
using Util.Path;

public class LobbyUI : MonoBehaviour
{
    private CharacterType _characterType;
    private GameObject[] _characterModels;

    public Transform SpawnPoint { get => _spawnPoint; set => _spawnPoint = value; }
    [SerializeField]
    private Transform _spawnPoint = null;
    [SerializeField] private GameObject _result;

    private int _currentCharacterIndex;

    private float _defaultVolume = 1f;
    public float DefaultVolume { get => _defaultVolume; }

    private TextMeshProUGUI _userName;

    private void Start() // 이보다 먼저 실행될 경우(Awake, OnEnable) 실행흐름에 영향
    {
        InitLobbyUISettings();
    }

    private void OnEnable()
    {
        _userName = GetComponentInChildren<TextMeshProUGUI>();
        if (GameManager.Instance.UserManager.UserData.Name == null)
        {
            return;
        }
        _userName.text = GameManager.Instance.UserManager.UserData.Name;
    }

    public void InitLobbyUISettings()
    {
        SetPanelAndButton(FilePath.LegendMenuUIPath, FilePath.LegendMenuButtonPath);
        SetPanelAndButton(FilePath.SettingUIPath, FilePath.SettingButtonPath);
        SetPanelAndButton(FilePath.MatchingUIPath, FilePath.MatchingButtonPath);
        SetLobbyCharaterModel();
    }

    private void SetLobbyCharaterModel()
    {
        _characterModels = new GameObject[(int)CharacterType.NumOfCharacter];
        _currentCharacterIndex = (int)CharacterType.Alice;
        for (int i = 0; i < (int)CharacterType.NumOfCharacter; ++i)
        {
            GameObject characterModelPrefab = Resources.Load<GameObject>(FilePath.GetLobbyCharacterPath((CharacterType)i));
            GameObject characterModelInstance = Instantiate(characterModelPrefab, _spawnPoint).gameObject;
            _characterModels[i] = characterModelInstance;
            characterModelInstance.transform.parent = _spawnPoint;
            characterModelInstance.SetActive(false);
        }
        _characterModels[_currentCharacterIndex].SetActive(true);
    }

    public void ChangeMainCharacter(int characterIndex)
    {
        _characterModels[_currentCharacterIndex].SetActive(false);
        _characterModels[characterIndex].SetActive(true);
        _characterType = (CharacterType)characterIndex;
        _currentCharacterIndex = characterIndex;
    }

    private void SetPanelAndButton(string panelPath, string buttonPath)
    {
        GameObject panelGameObject = Instantiate(Resources.Load<GameObject>(panelPath), transform.parent);
        GameObject buttonGameObject = Instantiate(Resources.Load<GameObject>(buttonPath), transform);

        IPanel panel = panelGameObject.GetComponent<IPanel>();
        EnablePanelButton button = buttonGameObject.GetComponent<EnablePanelButton>();

        panel.InitPanelSettings(this);
        button.InitEnablePanelButtonSettings(panelGameObject);
        panelGameObject.SetActive(false);
        if (buttonPath == FilePath.MatchingButtonPath)
        {
            button.Button.onClick.AddListener(GameManager.Instance.LobbyManager.Connect);
        }
    }

    public void ResetModelTransform()
    {
        foreach (GameObject characterModel in _characterModels)
        {
            Transform modelTransform = characterModel.transform;
            modelTransform.SetParent(_spawnPoint);
            modelTransform.localPosition = Vector3.zero;
            modelTransform.localScale = new Vector3(1, 1, 1);
            modelTransform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public GameObject GetCharacterModel(CharacterType characterType)
    {
        return _characterModels[(int)characterType];
    }
}

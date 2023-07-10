using System;
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

    private TextMeshProUGUI _userNameText;

    public event Action<CharacterType> OnCharacterChanged;

    private void Start()
    {
        SetUserName();

        Managers.LobbyManager.OnLogInSuccessed -= InitLobbyUISettings;
        Managers.LobbyManager.OnLogInSuccessed += InitLobbyUISettings;        
    }

    public void InitLobbyUISettings() // 패널 3개 생성 : 레전드 메뉴, 환경설정 메뉴, 매칭 UI
    {        
        SetUserName();
        SetPanelAndButton(ResourcesManager.LegendMenuUIPath, ResourcesManager.LegendMenuButtonPath);
        SetPanelAndButton(ResourcesManager.SettingUIPath, ResourcesManager.SettingButtonPath);
        SetPanelAndButton(ResourcesManager.MatchingUIPath, ResourcesManager.MatchingButtonPath);
        SetLobbyCharaterModel();
    }
    private void SetUserName()
    {
        _userNameText = GetComponentInChildren<TextMeshProUGUI>();
        string userNameInput = Managers.UserManager.UserLocalData.Name;

        if (userNameInput == null) return;

        _userNameText.text = userNameInput;
    }

    private void SetLobbyCharaterModel()
    {
        _characterModels = new GameObject[4];
        _currentCharacterIndex = (int)Managers.UserManager.UserLocalData.SelectedCharacter;

        for (int i = 0; i < 4; ++i)
        {
            if (i == 0) continue;
            GameObject characterModelPrefab = Resources.Load<GameObject>(FilePath.GetLobbyCharacterPath((CharacterType)i));
            GameObject characterModelInstance = Instantiate(characterModelPrefab, _spawnPoint).gameObject;
            _characterModels[i] = characterModelInstance;
            characterModelInstance.transform.parent = _spawnPoint;
            characterModelInstance.SetActive(false);
        }
        _characterModels[_currentCharacterIndex].SetActive(true);
    }

    public void ChangeLobbyCharacterModel(int characterIndex)
    {
        _characterModels[_currentCharacterIndex].SetActive(false);
        _characterModels[characterIndex].SetActive(true);
        _characterType = (CharacterType)characterIndex;
        _currentCharacterIndex = characterIndex;
        Managers.UserManager.UserLocalData.SetSelectedCharacter(_characterType);
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
        if (buttonPath == ResourcesManager.MatchingButtonPath)
        {
            button.Button.onClick.AddListener(Managers.LobbyManager.Connect);
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

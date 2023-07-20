using TMPro;
using UnityEngine;
using Util.Path;

public class LobbyUI : MonoBehaviour
{
    private LegendType _legendType;
    private GameObject[] _legendModels;

    public Transform SpawnPoint { get => _spawnPoint; set => _spawnPoint = value; }
    [SerializeField]
    private Transform _spawnPoint = null;
    [SerializeField] private GameObject _result;

    private int _currentCharacterIndex;

    private float _defaultVolume = 1f;
    public float DefaultVolume { get => _defaultVolume; }

    private TextMeshProUGUI _userNameText;        

    private void Start()
    {        
        SetUserName();

        Managers.LobbyManager.OnLogInSuccessed -= InitLobbyUISettings;
        Managers.LobbyManager.OnLogInSuccessed += InitLobbyUISettings;        
    }

    public void InitLobbyUISettings() // 패널 3개 생성 : 레전드 메뉴, 환경설정 메뉴, 매칭 UI
    {        
        SetUserName();
        SetPanelAndButton(FilePath.LegendMenuUIPath, FilePath.LegendMenuButtonPath);
        SetPanelAndButton(FilePath.SettingUIPath, FilePath.SettingButtonPath);
        SetPanelAndButton(FilePath.MatchingUIPath, FilePath.MatchingButtonPath);
        // TO DO : ResourceManager 및 UIManager에서 관리
        SetLobbyCharaterModel();
    }
    private void SetUserName()
    {
        _userNameText = GetComponentInChildren<TextMeshProUGUI>();
        string userNameInput = Managers.LobbyManager.UserLocalData.Name;

        if (userNameInput == null) return;

        _userNameText.text = userNameInput;
    }

    private void SetLobbyCharaterModel()
    {
        _legendModels = new GameObject[(int)LegendType.MaxCount];
        _currentCharacterIndex = (int)Managers.LobbyManager.UserLocalData.SelectedLegend;

        for (int i = 1; i < (int)LegendType.MaxCount; ++i)
        {            
            GameObject characterModelPrefab = Resources.Load<GameObject>(FilePath.GetLobbyLegendModelPath((LegendType)i));
            GameObject characterModelInstance = Instantiate(characterModelPrefab, _spawnPoint).gameObject;
            _legendModels[i] = characterModelInstance;
            characterModelInstance.transform.parent = _spawnPoint;
            characterModelInstance.SetActive(false);
        }
        _legendModels[_currentCharacterIndex].SetActive(true);
    }

    public void ChangeLobbyCharacterModel(int characterIndex)
    {
        _legendModels[_currentCharacterIndex].SetActive(false);
        _legendModels[characterIndex].SetActive(true);        
        _currentCharacterIndex = characterIndex;
        Managers.LobbyManager.UserLocalData.SelectedLegend = (LegendType)characterIndex;
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
            button.Button.onClick.AddListener(Managers.LobbyManager.Connect);
        }
    }

    public void ResetModelTransform()
    {
        foreach (GameObject legendModel in _legendModels)
        {
            Transform modelTransform = legendModel.transform;
            modelTransform.SetParent(_spawnPoint);
            modelTransform.localPosition = Vector3.zero;
            modelTransform.localScale = new Vector3(1, 1, 1);
            modelTransform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public GameObject GetLegendModel(LegendType characterType)
    {
        return _legendModels[(int)characterType];
    }
}

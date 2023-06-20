using UnityEngine;
using Util.Enum;
using Util.Path;

public class LobbyUI : MonoBehaviour
{
    private CharacterType _characterType;
    private GameObject[] _characterModels;
    [SerializeField] private Transform _spawnPoint;
    private int _currentCharacterIndex;

    private float _defaultVolume = 1f;
    public float DefaultVolume { get => _defaultVolume; }
    private void Awake()
    {
        InitLobbyUISettings();
    }
    public void InitLobbyUISettings()
    {
        SetPanelAndButton(FilePath.LegendMenuUIPath, FilePath.LegendMenuButtonPath);
        SetPanelAndButton(FilePath.SettingUIPath, FilePath.SettingButtonPath);
        SetPanelAndButton(FilePath.MatchingUIPath, FilePath.MatchingButtonPath);

        _characterModels = new GameObject[(int)CharacterType.NumOfCharacter];
        _currentCharacterIndex = (int)CharacterType.Alice;
        for (int i = 0; i < (int)CharacterType.NumOfCharacter; ++i)
        {
            GameObject _characterModelPrefab = Resources.Load<GameObject>(FilePath.GetLobbyCharacterPath((CharacterType)i));
            _characterModels[i] = Instantiate(_characterModelPrefab, _spawnPoint);
            _characterModels[i].SetActive(false);
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
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.Enum;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Experimental.GraphView;
using JetBrains.Annotations;
using UnityEditor.TextCore.Text;
using Util.Path;
using Cysharp.Threading.Tasks.Triggers;

public class ResultUI : MonoBehaviour
{
    //���� �ܺο��� �޾ƿ� ����. ����� �ٸ����� ����
    private CharacterType[] _characterType;
    private Transform[] _spawnPoints;
    [SerializeField] private TextMeshProUGUI _resultText;
    [SerializeField] private TextMeshProUGUI[] _playerIDText;
    private bool[] _isCharacterAlreadyUsed;
    private List<GameObject> _copiedModels;

    private Vector3 _fixedSize;
    private Quaternion _fixedRotation;
    private LobbyUI _lobbyUI;
    private int _maxPlayer = 2;

    private const string WinText = "�¸�";
    private const string DrawText = "���º�";
    private const string LoseText = "�й�";
    private int winHash = Animator.StringToHash("WinGame");
    private int loseHash = Animator.StringToHash("LoseGame");

    private Canvas _canvas;
    [SerializeField] private GameObject _characterSpawnPrefab;
    private GameObject _resultCharacter;
    public void InitSettings(LobbyUI lobbyUI)
    {
        _lobbyUI = lobbyUI;
        _canvas = GetComponent<Canvas>();
        if(_resultCharacter == null)
        {
            _resultCharacter = Instantiate(_characterSpawnPrefab);
            int numberOfSpawnPoints = _resultCharacter.transform.childCount - 1;
            _spawnPoints = new Transform[numberOfSpawnPoints];
            for (int i = 0; i < numberOfSpawnPoints; ++i)
            {
                _spawnPoints[i] = _resultCharacter.transform.GetChild(i);
            }
            _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            _canvas.worldCamera = _resultCharacter.transform.GetChild(numberOfSpawnPoints).GetComponent<Camera>();
        }
        else
        {
            _resultCharacter.SetActive(true);
        }
        _fixedRotation = Quaternion.Euler(0, 180, 0);
        _fixedSize = new Vector3(100, 100, 100);
    }


    private void Start()
    {
        //��ü�� �׽�Ʈ �ڵ�.
        UserData[] test = new UserData[_maxPlayer];
        test[0] = new UserData();
        test[1] = new UserData();
        test[0].Name = "�Ķ���";
        test[0].TeamType = TeamType.Red;
        test[1].Name = "�谩��";
        test[1].TeamType = TeamType.Blue;
        ShowResultUI(TeamType.Red, test);
    }
    //���� ������ ������ �� ����� �Լ�. �̺�Ʈ�� �����ؼ� �������.
    //���� �����͸� �迭 �������� �����Ͽ� 0���� ������ ����..?
    public void ShowResultUI(TeamType winningteam, UserData[] usersData)
    {
        _lobbyUI.gameObject.SetActive(false);
        UserData userData = usersData[0];
        if (userData.TeamType.Equals(winningteam))
        {
            _resultText.text = WinText;
        }
        else if (winningteam.Equals(TeamType.None))
        {
            _resultText.text = DrawText;
        }
        else
        {
            _resultText.text = LoseText;
        }
        _isCharacterAlreadyUsed = new bool[(int)CharacterType.NumOfCharacter];
        _copiedModels = new List<GameObject>();
        for (int i = 0; i < _maxPlayer; ++i)
        {
            //���� UserData���� �޾ƿ� ����
            //��, if(_isCharacterAlreadyUsed((int)usersData[i].Character))�� ����
            GameObject characterModel = new GameObject();

            if (_isCharacterAlreadyUsed[(int)CharacterType.Alice])
            {
                characterModel = Instantiate(Resources.Load<GameObject>(FilePath.GetLobbyCharacterPath(CharacterType.Alice)));
                _copiedModels.Add(characterModel);
            }
            else
            {
                characterModel = _lobbyUI.GetCharacterModel(CharacterType.Alice);
                //�׽�Ʈ �ڵ�. ���� UserData���� �޾ƿ� ����.
                _isCharacterAlreadyUsed[(int)CharacterType.Alice] = true;
            }

            Animator characterModelAnimator = characterModel.GetComponent<Animator>();
            characterModel.transform.SetParent(_spawnPoints[i]);
            characterModel.transform.localPosition = Vector3.zero;
            characterModel.transform.localScale = _fixedSize;
            characterModel.transform.rotation = _fixedRotation;

            _playerIDText[i].text = usersData[i].Name;

            if (usersData[i].TeamType.Equals(winningteam))
            {
                characterModelAnimator.SetTrigger(winHash);
            }
            else
            {
                characterModelAnimator.SetTrigger(loseHash);
            }
        }
    }
    private void OnDisable()
    {
        DisableCharacterTransform();
        ResetModelTransform();
        _lobbyUI.gameObject.SetActive(true);
    }

    private void DisableCharacterTransform() => _resultCharacter?.SetActive(false);

    private void ResetModelTransform()
    {
        if(_copiedModels == null)
        {
            return;
        }

        for(int i = 0; i < _copiedModels.Count; ++i)
        {
            Destroy(_copiedModels[i]);
        }
        _lobbyUI.ResetModelTransform();
    }
}

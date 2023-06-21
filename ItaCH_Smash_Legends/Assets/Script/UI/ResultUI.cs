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
    //추후 외부에서 받아올 예정. 현재는 앨리스로 통일
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

    private const string WinText = "승리";
    private const string DrawText = "무승부";
    private const string LoseText = "패배";
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
        //전체가 테스트 코드.
        UserData[] test = new UserData[_maxPlayer];
        test[0] = new UserData();
        test[1] = new UserData();
        test[0].Name = "파랑셩";
        test[0].TeamType = TeamType.Red;
        test[1].Name = "김갑수";
        test[1].TeamType = TeamType.Blue;
        ShowResultUI(TeamType.Red, test);
    }
    //추후 게임이 끝났을 때 실행될 함수. 이벤트에 구독해서 사용하자.
    //유저 데이터를 배열 형식으로 관리하여 0번에 본인을 삽입..?
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
            //추후 UserData에서 받아올 예정
            //즉, if(_isCharacterAlreadyUsed((int)usersData[i].Character))의 형식
            GameObject characterModel = new GameObject();

            if (_isCharacterAlreadyUsed[(int)CharacterType.Alice])
            {
                characterModel = Instantiate(Resources.Load<GameObject>(FilePath.GetLobbyCharacterPath(CharacterType.Alice)));
                _copiedModels.Add(characterModel);
            }
            else
            {
                characterModel = _lobbyUI.GetCharacterModel(CharacterType.Alice);
                //테스트 코드. 추후 UserData에서 받아올 예정.
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

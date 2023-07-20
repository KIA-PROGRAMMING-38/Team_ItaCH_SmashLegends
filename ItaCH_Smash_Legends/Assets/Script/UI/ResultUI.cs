using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Util.Path;

public class ResultUI : MonoBehaviour
{
    //추후 외부에서 받아올 예정. 현재는 앨리스로 통일
    private LegendType[] _legendType;
    private Transform[] _spawnPoints;
    [SerializeField] private TextMeshProUGUI _resultText;
    [SerializeField] private TextMeshProUGUI[] _playerIDText;
    private bool[] _isLegendUsedInLobby;
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
    [SerializeField] private GameObject _legendSpawnPrefab;
    private GameObject _resultPanelLegendModel;
    public void InitSettings(LobbyUI lobbyUI)
    {
        _lobbyUI = lobbyUI;
        _canvas = GetComponent<Canvas>();
        if (_resultPanelLegendModel == null)
        {
            _resultPanelLegendModel = Instantiate(_legendSpawnPrefab);
            int numberOfSpawnPoints = _resultPanelLegendModel.transform.childCount - 1;
            _spawnPoints = new Transform[numberOfSpawnPoints];
            for (int i = 0; i < numberOfSpawnPoints; ++i)
            {
                _spawnPoints[i] = _resultPanelLegendModel.transform.GetChild(i);
            }
            _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            _canvas.worldCamera = _resultPanelLegendModel.transform.GetChild(numberOfSpawnPoints).GetComponent<Camera>();
        }
        else
        {
            _resultPanelLegendModel.SetActive(true);
        }
        _fixedRotation = Quaternion.Euler(0, 180, 0);
        _fixedSize = new Vector3(100, 100, 100);
    }


    //private void Start()
    //{
    //    //전체가 테스트 코드.
    //    UserData[] test = new UserData[_maxPlayer];
    //    test[0] = new UserData();
    //    test[1] = new UserData();
    //    test[0].Name = "파랑셩";
    //    test[0].TeamType = TeamType.Red;
    //    test[1].Name = "김갑수";
    //    test[1].TeamType = TeamType.Blue;
    //    ShowResultUI(TeamType.Red, test);
    //}
    //추후 게임이 끝났을 때 실행될 함수. 이벤트에 구독해서 사용하자.
    //유저 데이터를 배열 형식으로 관리하여 0번에 본인을 삽입..?
    public void ShowResultUI(TeamType winningteam, UserData[] users)
    {
        _lobbyUI.gameObject.SetActive(false);
        UserData user = users[0];
        if (user.Team.Type.Equals(winningteam))
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
        _copiedModels = new List<GameObject>();
        for (int i = 0; i < _maxPlayer; ++i)
        {
            //추후 UserData에서 받아올 예정
            //즉, if(_isCharacterAlreadyUsed((int)usersData[i].Character))의 형식
            GameObject legendModel = new GameObject();

            if (_isLegendUsedInLobby[(int)LegendType.Alice])
            {
                legendModel = Instantiate(Resources.Load<GameObject>(FilePath.GetLobbyLegendModelPath(LegendType.Alice)));
                _copiedModels.Add(legendModel);
            }
            else
            {
                legendModel = _lobbyUI.GetLegendModel(LegendType.Alice);
                //테스트 코드. 추후 UserData에서 받아올 예정.
                _isLegendUsedInLobby[(int)LegendType.Alice] = true;
            }

            Animator legendModelAnimator = legendModel.GetComponent<Animator>();
            legendModel.transform.SetParent(_spawnPoints[i]);
            legendModel.transform.localPosition = Vector3.zero;
            legendModel.transform.localScale = _fixedSize;
            legendModel.transform.rotation = _fixedRotation;

            _playerIDText[i].text = users[i].Name;

            if (users[i].Team.Type.Equals(winningteam))
            {
                legendModelAnimator.SetTrigger(winHash);
            }
            else
            {
                legendModelAnimator.SetTrigger(loseHash);
            }
        }
    }
    private void OnDisable()
    {
        DisableLegendTransform();
        ResetModelTransform();
        _lobbyUI.gameObject.SetActive(true);
    }

    private void DisableLegendTransform() => _resultPanelLegendModel?.SetActive(false);

    private void ResetModelTransform()
    {
        if (_copiedModels == null)
        {
            return;
        }

        for (int i = 0; i < _copiedModels.Count; ++i)
        {
            Destroy(_copiedModels[i]);
        }
        _lobbyUI.ResetModelTransform();
    }
}

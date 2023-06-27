using Photon.Pun;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchUI : MonoBehaviourPunCallbacks, IPanel
{
    private bool[] _isPlayerMatched;
    private int _maxPlayer;
    private int _currentMatchedPlayer;
    [SerializeField] private MatchBox[] _matchBoxes;
    [SerializeField] private MatchIcon _matchIcon;
    [SerializeField] private TextMeshProUGUI _matchText;
    [SerializeField] private Button _removePanelButton;

    public event Action _OnStageStart;

    //private float _time;
    public bool IsGameStarted { get => _isGameStarted; set => _isGameStarted = value; }
    private bool _isGameStarted;

    public override void OnEnable()
    {
        GameManager.Instance.LobbyManager.ConnectionInfoText = _matchText;

        if (_isGameStarted)
        {
            Debug.Log("connect 실행");
            GameManager.Instance.LobbyManager.Connect();
        }
    }

    public void InitPanelSettings(LobbyUI lobbyUI)
    {
        //추후 모드에서 최대 인원수를 가져올 예정.
        _maxPlayer = 2;
        _isPlayerMatched = new bool[_maxPlayer];
        _currentMatchedPlayer = 0;
        for (int i = 0; i < _maxPlayer; ++i)
        {
            _matchBoxes[i].InitMatchBoxSettings();
        }
        _matchIcon.InitMatchIconSettings();
        _OnStageStart -= _matchIcon.SetMatchCompleteImage;
        _OnStageStart += _matchIcon.SetMatchCompleteImage;
        _OnStageStart -= () => _removePanelButton.enabled = false;
        _OnStageStart += () => _removePanelButton.enabled = false;
    }

    //추후 포톤 로직으로 변경 예정
    //public void Update()
    //{
    //    if (_time >= 3)
    //    {
    //        int random = UnityEngine.Random.Range(0,3);
    //        switch(random)
    //        {
    //            case 1:
    //                _isPlayerMatched[0] = true;
    //                _isPlayerMatched[1] = false;
    //                break;
    //            case 2:
    //                _isPlayerMatched[0] = true;
    //                _isPlayerMatched[1] = true;
    //                break;
    //            default:
    //                _isPlayerMatched[0] = false;
    //                _isPlayerMatched[1] = false;
    //                break;
    //        }
    //        for(int i = 0; i < _maxPlayer; ++i)
    //        {
    //            SetBox(_isPlayerMatched[i], _matchBoxes[i]);
    //        }
    //        _time = 0;
    //    }
    //    else
    //    {
    //        _time += Time.deltaTime;
    //    }

    //}

    private void OnDestroy()
    {
        _OnStageStart -= _matchIcon.SetMatchCompleteImage;
        _OnStageStart -= () => _removePanelButton.enabled = false;
    }

    public void SetBox(bool isMatched, MatchBox _matchBox)
    {
        if (isMatched)
        {
            _matchBox.StartBoxGlow();
            _currentMatchedPlayer = Mathf.Min(++_currentMatchedPlayer, _maxPlayer);
        }
        else
        {
            _matchBox.EndBoxGlow();
            _currentMatchedPlayer = Mathf.Max(0, --_currentMatchedPlayer);
        }

        if (_currentMatchedPlayer.Equals(_maxPlayer))
        {
            StartStage();
        }
    }

    public void StartStage()
    {
        _matchText.text = "게임이 시작됩니다! 준비하세요!";
        _OnStageStart.Invoke();
    }
}

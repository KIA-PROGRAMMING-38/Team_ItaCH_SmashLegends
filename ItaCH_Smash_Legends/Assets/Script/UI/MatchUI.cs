using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class MatchUI : MonoBehaviour, IPanel
{
    private bool[] _isPlayerMatched;
    private int _maxPlayer;
    private int _currentMatchedPlayer;
    [SerializeField] private MatchBox[] _matchBoxes;
    [SerializeField] private MatchIcon _matchIcon;
    [SerializeField] private TextMeshProUGUI _matchText;
    [SerializeField] private Button _removePanelButton;

    public event Action _OnStageStart;

    private float _time;

    public void InitPanelSettings(LobbyUI lobbyUI)
    {
        //���� ��忡�� �ִ� �ο����� ������ ����.
        _maxPlayer = 2;
        _isPlayerMatched = new bool[_maxPlayer];
        _currentMatchedPlayer = 0;
        for(int i = 0; i < _maxPlayer; ++i)
        {
            _matchBoxes[i].InitMatchBoxSettings();
        }
        _matchIcon.InitMatchIconSettings();
        _OnStageStart -= _matchIcon.SetMatchCompleteImage;
        _OnStageStart += _matchIcon.SetMatchCompleteImage;
        _OnStageStart -= () => _removePanelButton.enabled = false;
        _OnStageStart += () => _removePanelButton.enabled = false;
    }

    //���� ���� �������� ���� ����
    public void Update()
    {
        if (_time >= 3)
        {
            int random = UnityEngine.Random.Range(0,3);
            switch(random)
            {
                case 1:
                    _isPlayerMatched[0] = true;
                    _isPlayerMatched[1] = false;
                    break;
                case 2:
                    _isPlayerMatched[0] = true;
                    _isPlayerMatched[1] = true;
                    break;
                default:
                    _isPlayerMatched[0] = false;
                    _isPlayerMatched[1] = false;
                    break;
            }
            for(int i = 0; i < _maxPlayer; ++i)
            {
                SetBox(_isPlayerMatched[i], _matchBoxes[i]);
            }
            _time = 0;
        }
        else
        {
            _time += Time.deltaTime;
        }

    }

    private void OnDestroy()
    {
        _OnStageStart -= _matchIcon.SetMatchCompleteImage;
        _OnStageStart -= () => _removePanelButton.enabled = false;
    }

    public void SetBox(bool isMatched, MatchBox _matchBox)
    {
        if(isMatched)
        {
            _matchBox.StartBoxGlow();
            _currentMatchedPlayer = Mathf.Min(++_currentMatchedPlayer, _maxPlayer);
        }
        else
        {
            _matchBox.EndBoxGlow();
            _currentMatchedPlayer = Mathf.Max(0, --_currentMatchedPlayer);
        }

        if(_currentMatchedPlayer.Equals(_maxPlayer))
        {
            StartStage();
        }
    }

    public void StartStage()
    {
        _matchText.text = "������ ���۵˴ϴ�! �غ��ϼ���!";
        _OnStageStart.Invoke();
    }
}
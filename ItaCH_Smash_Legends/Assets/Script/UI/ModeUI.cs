using Cysharp.Threading.Tasks.Triggers;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModeUI : MonoBehaviour
{
    [Header("UI Images in Prefab")]
    [SerializeField] private HealthBar[] _healthPointBars;
    [SerializeField] private ScoreSet[] _scoreSets;
    [SerializeField] private Timer _timer;

    private StageManager _stageManager;
    public void InitModeUISettings(StageManager stageManager)
    {
        _stageManager = stageManager;
        GameObject[] players = stageManager.Players;
        for (int i = 0; i < players.Length - 1; ++i)
        {
            _healthPointBars[i].InitHealthBarSettings(players[i + 1].GetComponent<CharacterStatus>());
            _scoreSets[i].InitScoreSetSettings((TeamType)(i + 1));
            BindEventWithScoreSets(i);
        }
        _timer.InitTimerSettings();
        BindEventWithTimer();
    }
    public void BindEventWithScoreSets(int i)
    {
        _stageManager.OnTeamScoreChanged -= _scoreSets[i].GetScore;
        _stageManager.OnTeamScoreChanged += _scoreSets[i].GetScore;
    }
    public void BindEventWithTimer()
    {
        _stageManager.OnTimeChange -= _timer.ChangeTime;
        _stageManager.OnTimeChange += _timer.ChangeTime;
    }

    public void OnDestroy()
    {
        for (int i = 0; i < _scoreSets.Length; ++i)
        {
            _stageManager.OnTeamScoreChanged -= _scoreSets[i].GetScore;
        }
    }
}

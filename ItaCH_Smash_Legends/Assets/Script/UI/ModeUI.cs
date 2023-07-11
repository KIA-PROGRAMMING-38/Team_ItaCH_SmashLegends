using TMPro;
using UnityEngine;

public class ModeUI : MonoBehaviour
{
    [Header("UI Images in Prefab")]
    [SerializeField] private HealthBar[] _healthPointBars;
    [SerializeField] private ScoreSet[] _scoreSets;
    [SerializeField] private Portrait[] _portraits;
    [SerializeField] private Timer _timer;
    [SerializeField] private RespawnTimer _respawnTimer;
    [SerializeField] private TextMeshProUGUI[] _userName;

    private StageManager _stageManager;
    public void InitModeUISettings(StageManager stageManager)
    {
        _stageManager = stageManager;
        GameObject[] players = stageManager.Players;

        SetUIForEachPlayers(players);
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
    public void BindEventWithRespawnUI(CharacterStatus characterStatus)
    {
        characterStatus.OnPlayerDie -= _respawnTimer.CheckPlayer;
        characterStatus.OnPlayerDie += _respawnTimer.CheckPlayer;
    }
    public void BindEventWithPortraits(CharacterStatus characterStatus, Portrait portrait)
    {
        characterStatus.OnPlayerDie -= portrait.StartRespawnTimer;
        characterStatus.OnPlayerDie += portrait.StartRespawnTimer;
    }
    public void BindEventWithHealthBar(CharacterStatus characterStatus, HealthBar healthBar)
    {
        characterStatus.OnPlayerHealthPointChange -= healthBar.SetHealthPoint;
        characterStatus.OnPlayerHealthPointChange += healthBar.SetHealthPoint;
    }
    private void SetUIForEachPlayers(GameObject[] players)
    {
        for (int i = 0; i < players.Length; ++i)
        {
            CharacterStatus characterStatus = players[i].GetComponent<CharacterStatus>();
            _healthPointBars[i].InitHealthBarSettings();
            BindEventWithHealthBar(characterStatus, _healthPointBars[i]);

            Sprite characterPortrait = Resources.Load<Sprite>(Util.Path.FilePath.GetCharacterSpritePath(characterStatus.Character));
            _portraits[i].InitPortraitSetting(characterPortrait);
            BindEventWithPortraits(characterStatus, _portraits[i]);

            _scoreSets[i].InitScoreSetSettings((TeamType)(i + 1));
            BindEventWithScoreSets(i);

            SetUIForCurrentPlayer(characterStatus);
        }
    }

    private void SetUIForCurrentPlayer(CharacterStatus characterStatus)
    {
        _timer.InitTimerSettings();
        _respawnTimer.InitRespawnTimerSettings(characterStatus);
        SetPlayerName(characterStatus);
        BindEventWithRespawnUI(characterStatus);
        BindEventWithTimer();
    }

    private void SetPlayerName(CharacterStatus playerData)
    {
        _userName[playerData.PlayerID].text = playerData.Name;
    }

    public void OnDestroy()
    {
        for (int i = 0; i < _scoreSets.Length; ++i)
        {
            _stageManager.OnTeamScoreChanged -= _scoreSets[i].GetScore;
        }
    }
}

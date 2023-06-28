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
    [SerializeField] private TextMeshProUGUI _1PUserName;
    [SerializeField] private TextMeshProUGUI _2PUserName;

    private StageManager _stageManager;
    public void InitModeUISettings(StageManager stageManager)
    {
        _stageManager = stageManager;
        GameObject[] players = stageManager.Players;
        //실제로 조작을 하는 플레이어의 characterStatus
        _1PUserName.text = GameManager.Instance.UserManager.UserLocalData.Name;
        CharacterStatus playerCharacterStatus = players[1].GetComponent<CharacterStatus>();
        SetUIForEachPlayers(players);
        SetUIForCurrentPlayer(playerCharacterStatus);
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
        for (int i = 0; i < players.Length - 1; ++i)
        {
            CharacterStatus characterStatus = players[i + 1].GetComponent<CharacterStatus>();

            _healthPointBars[i].InitHealthBarSettings();
            BindEventWithHealthBar(characterStatus, _healthPointBars[i]);

            Sprite characterPortrait = Resources.Load<Sprite>(Util.Path.FilePath.GetCharacterSpritePath(characterStatus.CharacterType));
            _portraits[i].InitPortraitSetting(characterPortrait);
            BindEventWithPortraits(characterStatus, _portraits[i]);

            _scoreSets[i].InitScoreSetSettings((TeamType)(i + 1));
            BindEventWithScoreSets(i);
        }
    }

    private void SetUIForCurrentPlayer(CharacterStatus characterStatus)
    {
        _timer.InitTimerSettings();
        _respawnTimer.InitRespawnTimerSettings(characterStatus);
        BindEventWithRespawnUI(characterStatus);
        BindEventWithTimer();
    }
    public void OnDestroy()
    {
        for (int i = 0; i < _scoreSets.Length; ++i)
        {
            _stageManager.OnTeamScoreChanged -= _scoreSets[i].GetScore;
        }
    }
}

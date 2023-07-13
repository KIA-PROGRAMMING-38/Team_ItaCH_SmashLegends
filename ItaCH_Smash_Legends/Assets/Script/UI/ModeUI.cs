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
        //CharacterStatus[] players = stageManager.Players; // TODO 로직 변경

        //SetUIForEachPlayers(players);
    }
    public void BindEventWithScoreSets(int i)
    {
        //_stageManager.OnTeamScoreChanged -= _scoreSets[i].GetScore;
        //_stageManager.OnTeamScoreChanged += _scoreSets[i].GetScore; // TODO : 팀스코어 판별 로직
    }
    public void BindEventWithTimer()
    {
        _stageManager.OnTimeChange -= _timer.ChangeTime;
        _stageManager.OnTimeChange += _timer.ChangeTime;
    }
    public void BindEventWithRespawnUI(CharacterStatus characterStatus)
    {
        //characterStatus.OnPlayerDie -= _respawnTimer.CheckPlayer;
        //characterStatus.OnPlayerDie += _respawnTimer.CheckPlayer; // TO DO : 레전드 컨트롤러에서 리스폰 정의
    }
    public void BindEventWithPortraits(CharacterStatus characterStatus, Portrait portrait)
    {
        //characterStatus.OnPlayerDie -= portrait.StartRespawnTimer;
        //characterStatus.OnPlayerDie += portrait.StartRespawnTimer;
    }
    public void BindEventWithHealthBar(CharacterStatus characterStatus, HealthBar healthBar)
    {
        characterStatus.OnPlayerHealthPointChange -= healthBar.SetHealthPoint;
        characterStatus.OnPlayerHealthPointChange += healthBar.SetHealthPoint;
    }
    private void SetUIForEachPlayers(LegendController[] players)
    {
        for (int i = 0; i < players.Length; ++i)
        {
            CharacterStatus characterStatus = players[i].GetComponent<CharacterStatus>();
            _healthPointBars[i].InitHealthBarSettings();
            BindEventWithHealthBar(characterStatus, _healthPointBars[i]);

            //Sprite characterPortrait = Resources.Load<Sprite>(Util.Path.FilePath.GetCharacterSpritePath(characterStatus.Character)); TODO : 플레이어가 선택한 캐릭터 정보 연결
            Sprite characterPortrait = Resources.Load<Sprite>(Util.Path.FilePath.GetCharacterSpritePath(Util.Enum.CharacterType.Peter)); // 임시 >> 수정 이후 삭제           
            _portraits[i].InitPortraitSetting(characterPortrait);
            BindEventWithPortraits(characterStatus, _portraits[i]);

            _scoreSets[i].InitScoreSetSettings((TeamType)(i + 1));
            BindEventWithScoreSets(i);

            SetUIForCurrentPlayer(i, characterStatus.Name, characterStatus);
        }
    }

    private void SetUIForCurrentPlayer(int userID, string userName, CharacterStatus characterStatus)
    {
        _timer.InitTimerSettings();
        _respawnTimer.InitRespawnTimerSettings(characterStatus);
        SetPlayerName(userID, userName);
        BindEventWithRespawnUI(characterStatus);
        BindEventWithTimer();
    }

    private void SetPlayerName(int userID, string userName)
    {
        _userName[userID].text = userName;
    }

    public void OnDestroy()
    {
        for (int i = 0; i < _scoreSets.Length; ++i)
        {
            //_stageManager.OnTeamScoreChanged -= _scoreSets[i].GetScore; // TODO : 팀스코어 판별 로직
        }
    }
}

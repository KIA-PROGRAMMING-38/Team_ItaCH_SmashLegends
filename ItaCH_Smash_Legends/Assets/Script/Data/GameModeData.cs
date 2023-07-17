using UnityEngine;

public class GameModeData
{
    public GameModeType GameModeType { get; set; } // DataManager 에서 가져오려면 수정 필요
    public GameObject Map { get; set; } // DataManger 에서 가져오려면 수정 필요
    public Transform[] SpawnPoints { get; set; } // DataManager 에서 가져오려면 수정 필요
    // 위 세개는 추상 클래스가 가지고 있고 아래만 데이터로
    public int MaxPlayer { get; set; }
    public int MaxTeamCount { get; set; }
    public int MaxTeamMember { get; set; }
    public int TimeLimits { get; set; }
    public int WinningScore { get; set; }
    public float ModeDefaultRespawnTime { get; set; }
}
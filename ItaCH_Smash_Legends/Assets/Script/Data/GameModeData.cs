using UnityEngine;

public class GameModeData
{
    public GameModeType GameModeType { get; set; }
    public Transform[] SpawnPoints { get; set; }
    // 위 세개는 상위 클래스가 필드로 가지고 있고 아래만 데이터로
    public string Map { get; set; }
    public int MaxPlayer { get; set; }
    public int MaxTeamCount { get; set; }
    public int MaxTeamMember { get; set; }
    public int TimeLimits { get; set; }
    public int WinningScore { get; set; }
    public float ModeDefaultRespawnTime { get; set; }
}
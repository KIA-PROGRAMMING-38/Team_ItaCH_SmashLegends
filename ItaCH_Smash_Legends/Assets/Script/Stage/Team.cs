using System.Collections.Generic;

public class Team
{
    public List<UserData> Members { get; private set; } = new();
    public int Score { get; private set; }
    public TeamType Type { get; set; }

    public void AddMember(UserData user)
    {
        Members.Add(user);
        user.Team = this;
    }

    public void GetScore()
    {
        ++Score; // 플레이어 사망 시 실행
        if (Score == Managers.StageManager.CurrentGameMode.WinningScore)
        {
            Managers.StageManager.EndGame(this.Type);
        } // 이벤트 처리 게임 모드가 구독 
        // 게임이 종료되었다는 것도 이벤트 
        // StageManager가 Timer 멈추고 결과 넘어가고 이런거도 이벤트
    }
}
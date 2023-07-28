using System.Collections.Generic;

public class Team
{
    public List<UserData> Members 
    {
        get
        {
            if (Members.Count == 0)
            {
                Members = new List<UserData>();
                Members.Add(new UserData());
            }           
            return Members;
        }
        private set => Members = value; 
    }
    public TeamType Type { get; set; }
    public int Score { get; private set; }

    public void AddMember(UserData user)
    {
        Members.Add(user);
        user.TeamType = this.Type;
    }
  
    public void RemoveMember(UserData user)
    {
        Members.Remove(user);
    }

    public void GetScore()
    {
        ++Score; // 플레이어 사망 시 실행
        if (Score == Managers.StageManager.CurrentGameMode.WinningScore)
        {
            Managers.StageManager.CurrentGameMode.IsOver();
        }        
    }
}
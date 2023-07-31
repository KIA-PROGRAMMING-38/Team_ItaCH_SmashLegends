using System.Collections.Generic;
using UnityEngine;

public class Team
{
    public List<UserData> Members { get; private set; }
    public TeamType Type { get; set; }
    public int Score { get; private set; }

    public void Init()
    {
        Members= new List<UserData>();
    }

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
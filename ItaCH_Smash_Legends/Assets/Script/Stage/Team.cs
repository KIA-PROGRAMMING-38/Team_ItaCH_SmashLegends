using System.Collections.Generic;
using UnityEngine;

public class Team
{
    public List<UserData> Members { get; private set; }
    public TeamType Type { get; set; }
    public int Score { get; private set; }

    public void Init()
    {
        Members = new List<UserData>();
        Type = (TeamType)Managers.StageManager.CurrentGameMode.Teams.Count;
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
        ++Score;
        Managers.UIManager.FindPopup<UI_DuelModePopup>().RefreshPopupUI();

        if (Score == Managers.StageManager.CurrentGameMode.WinningScore)
        {
            Managers.StageManager.CurrentGameMode.IsOver();
        }        
    }

    public void InitDefaultTeam()
    {
        Init();
        for (int id = 0; id < Managers.StageManager.CurrentGameMode.MaxTeamMember; ++id)
        {
            UserData dummyUserData = new UserData();
            dummyUserData.GetDefaultUserData(id);            
            AddMember(dummyUserData);
        }
    }
}
public class Team
{
    public UserData[] Members { get; set; }
    public int Score { get; private set; }
    public int MemberCount { get => _memberCount; }
    private int _memberCount = 0;
    public TeamType TeamColor { get; set; }

    public void Init(int teamSize)
    {
        Members = new UserData[teamSize];
    }
    public void AddMember(UserData user)
    {
        UnityEngine.Debug.Log($"{user}");
        Members[_memberCount] = user;
        user.Team = TeamColor;
        ++_memberCount;
    }
    public void GetScore() => ++Score; // 플레이어 사망 시 실행    
}
public class Team
{
    public UserData[] Users { get; set; }
    public int Score { get; private set; }
    public int MemberCount { get => _memberCount; }
    private int _memberCount = 0;

    public void Init(int teamSize)
    {
        UserData[] Users = new UserData[teamSize];
    }
    public void AddMember(UserData user)
    {        
        Users[_memberCount] = user;
        ++_memberCount;
    }
    public void GetScore() => ++Score;
}
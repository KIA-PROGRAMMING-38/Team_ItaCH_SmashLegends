public class GameRoomManager
{
    private const int MAXIMUM_PLAYER = 4;
    public UserData[] AllUserDatas = new UserData[MAXIMUM_PLAYER];
    public UserData UserLocalData { get; set; }

    public void Init()
    {
        UserLocalData = new UserData();
        UserLocalData.SelectedLegend = LegendType.None;
        Managers.LobbyManager.OnUpdateUserDatas -= ResiterUserData;
        Managers.LobbyManager.OnUpdateUserDatas += ResiterUserData;
    }

    public UserData GetUserData(int userID)
    {
        if (AllUserDatas[userID] == null)
        {
            AllUserDatas[userID] = GetDefaultUserData(userID);
        }
        return AllUserDatas[userID];
    }

    private UserData GetDefaultUserData(int id)
    {
        UserData defaultUserData = new UserData();
        defaultUserData.Name = $"Bot{id}";
        defaultUserData.ID = id;
        defaultUserData.SelectedLegend = LegendType.None;        
        return defaultUserData;
    }

    private void ResiterUserData(int id, UserData userLocalData)
    {
        AllUserDatas[id] = userLocalData;
    }

    private void ClearUserData(int id)
    {
        AllUserDatas[id] = default;
    }
}


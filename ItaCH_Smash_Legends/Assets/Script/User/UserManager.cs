using UnityEngine;
using Util.Enum;

public class UserManager
{
    private const int MAXIMUM_PLAYER = 4;
    public UserData[] AllUserDatas = new UserData[MAXIMUM_PLAYER];
    public UserData UserLocalData { get => _userLocalData; set => _userLocalData = value; }
    private UserData _userLocalData;

    public void Init()
    {
        _userLocalData = new UserData();
        _userLocalData.SelectedCharacter = CharacterType.None;
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
        defaultUserData.Id = id;
        defaultUserData.SelectedCharacter = CharacterType.None;
        defaultUserData.Team = TeamType.None;
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


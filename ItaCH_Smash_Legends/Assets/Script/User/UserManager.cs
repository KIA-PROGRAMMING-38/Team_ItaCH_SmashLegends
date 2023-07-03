using UnityEngine;
using Util.Enum;

public class UserManager : MonoBehaviour
{
    private const int MAXIMUM_PLAYER = 4;
    public UserData[] AllUserDatas = new UserData[MAXIMUM_PLAYER];
    public UserData UserLocalData { get => _userLocalData; set => _userLocalData = value; }
    private UserData _userLocalData;

    void Awake()
    {
        _userLocalData = new UserData();
        _userLocalData.SelectedCharacter = CharacterType.None;
        GameManager.Instance.LobbyManager.OnUpdateUserDatas -= ResiterUserData;
        GameManager.Instance.LobbyManager.OnUpdateUserDatas += ResiterUserData;
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
        defaultUserData.TeamType = TeamType.None;
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


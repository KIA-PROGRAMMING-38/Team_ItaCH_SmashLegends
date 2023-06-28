using UnityEngine;

public class UserManager : MonoBehaviour
{
    private const int MAXIMUM_PLAYER = 4;
    public UserData[] AllUserDatas = new UserData[MAXIMUM_PLAYER];
    public UserData UserLocalData { get => _userData; set => _userData = value; }
    private UserData _userData;
    void Start()
    {
        _userData = new UserData();
    }
    public UserData GetUserData(int userID)
    {
        if (AllUserDatas[userID] == null)
        {
            AllUserDatas[userID] = new UserData();
        }
        return AllUserDatas[userID];
    }
}

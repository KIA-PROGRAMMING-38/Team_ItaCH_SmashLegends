using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using System;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    // 서버 접속 이벤트 >> LogIn UI 화면
    public event Action OnConnectingtoServer;
    public event Action OnDisconnectedfromServer;
    public event Action OnLogInSuccessed;

    // 룸 매칭 이벤트 >> Match UI 화면
    public event Action OnJoiningRoom;
    public event Action OnCreatingRoom;
    public event Action OnWaitingPlayer;
    public event Action OnMatchingSuccess;
    public event Action<GameMode> OnInGameSceneLoaded;

    public event Action<int, UserData> OnUpdateUserDatas;

    public void Init()
    {

    }

    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        OnConnectingtoServer?.Invoke();
    }

    public override void OnConnectedToMaster() => OnLogInSuccessed.Invoke();

    public override void OnDisconnected(DisconnectCause cause)
    {
        OnDisconnectedfromServer?.Invoke();
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            OnJoiningRoom?.Invoke();
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            OnDisconnectedfromServer?.Invoke();
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        OnCreatingRoom?.Invoke();
        int totalPlayer = Managers.StageManager.CurrentGameMode.MaxPlayer;
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = totalPlayer });
    }

    public override void OnJoinedRoom()
    {
        OnWaitingPlayer?.Invoke();
        ResisterUserLocalData();
        MatchWithBot();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 상대 데이터 받아오는 부분        
    }

    private void EnterInGameScene()
    {
        PhotonNetwork.LoadLevel(StringLiteral.INGAME);
        OnMatchingSuccess?.Invoke();
    }

    private enum Level
    {
        Lobby,
        Ingame
    }

    public void OnLevelWasLoaded(int level)
    {
        switch (level)
        {
            case (int)Level.Lobby:
                return;

            case (int)Level.Ingame:
                GameMode currentGameMode = Managers.StageManager.CurrentGameMode;
                OnInGameSceneLoaded?.Invoke(currentGameMode);
                return;
        }
    }

    private void ResisterUserLocalData()
    {
        int enteringOrder = GetEnteringOrder();
        UserData userLocalData = GetUserLocalData();
        userLocalData.ID = enteringOrder;
        OnUpdateUserDatas?.Invoke(enteringOrder, userLocalData);
    }

    private int GetEnteringOrder()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            return 0;
        }
        return 1; // 4인 모드 고려 시 수정 필요
    }

    private UserData GetUserLocalData()
    {
        UserData userLocalData = Managers.UserManager.UserLocalData;
        return userLocalData;
    }

    private async UniTask MatchWithBot()
    {
        await UniTask.Delay(2000); // 현재 2초 동안 매칭 안 잡히면 연습장 자동 입장
        EnterInGameScene();
    }
}
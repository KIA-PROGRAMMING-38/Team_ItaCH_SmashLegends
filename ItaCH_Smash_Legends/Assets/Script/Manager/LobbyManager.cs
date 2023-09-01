using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public UserData UserLocalData = new UserData();

    // 서버 접속 이벤트 >> LogIn UI 화면
    public event Action OnConnectingtoServer;
    public event Action OnDisconnectedfromServer;
    public event Action OnLogInSuccessed;

    // 룸 매칭 이벤트 >> Match UI 화면
    public event Action OnJoiningRoom;
    public event Action OnCreatingRoom;
    public event Action OnWaitingPlayer;
    public event Action<UserData> OnUpdatePlayerList;
    public event Action OnMatchingSuccess;
    public event Action OnInGameSceneLoaded;

    public event Action<int, UserData> OnUpdateUserDatas;

    public void Init()
    {

    }

    private void Start()
    {
        Managers.SoundManager.Play(SoundType.BGM, StringLiteral.BGM_TITLE);
    }

    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        OnConnectingtoServer?.Invoke();
    }

    public override void OnConnectedToMaster() => OnLogInSuccessed?.Invoke();

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
        Managers.SoundManager.Play(SoundType.SFX,StringLiteral.SFX_GAMEMODESTART);
        Managers.SoundManager.Play(SoundType.BGM,StringLiteral.BGM_MATCH);

        int totalPlayer = Managers.StageManager.CurrentGameMode.MaxPlayer;
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = totalPlayer });
    }

    public override void OnJoinedRoom()
    {
        OnWaitingPlayer?.Invoke();
        SetUserID();
        OnUpdatePlayerList?.Invoke(UserLocalData);

        MatchWithBot();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 상대 데이터 받아오는 부분
    }

    private void EnterInGameScene()
    {
        PhotonNetwork.LoadLevel(StringLiteral.INGAME);
        Managers.SoundManager.Play(SoundType.BGM, StringLiteral.BGM_STAGE); 
        Managers.SoundManager.Play(SoundType.SFX,StringLiteral.SFX_MATCH_START);
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
                OnInGameSceneLoaded?.Invoke();
                return;
        }
    }

    private void SetUserID()
    {
        int enteringOrder = GetEnteringOrder();
        UserLocalData.ID = enteringOrder;
    }

    private int GetEnteringOrder()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            return 0;
        }
        return 1; // 4인 모드 고려 시 수정 필요
    }

    private async UniTask MatchWithBot()
    {
        OnUpdatePlayerList(GetDefaultUserData(UserLocalData.ID + 1));
        await UniTask.Delay(2000); // 현재 2초 동안 매칭 안 잡히면 연습장 자동 입장
        EnterInGameScene();
    }

    private UserData GetDefaultUserData(int id)
    {
        UserData defaultUserData = new UserData();
        defaultUserData.Name = $"Bot{id}";
        defaultUserData.ID = id;
        defaultUserData.SelectedLegend = LegendType.None;
        return defaultUserData;
    }
}
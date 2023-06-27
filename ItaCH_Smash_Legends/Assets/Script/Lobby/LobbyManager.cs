using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string _gameVersion = "1";
    public GameObject LobbyUI { get => _lobbyUI; set => _lobbyUI = value; }
    private GameObject _lobbyUI;

    public TextMeshProUGUI ConnectionInfoText { get => _connectionInfoText; set => _connectionInfoText = value; } // 로딩 패널에서 현재 접속 상태를 보여줄 텍스트    
    private TextMeshProUGUI _connectionInfoText;
    public Button ExitRoomButton { get => _exitRoomButton; set => _exitRoomButton = value; }
    private Button _exitRoomButton;

    public event Action OnLogInSuccess;
    public event Action<GameModeType> OnMatchSuccess;
    public event Action<GameMode> OnEnteringGameMode;

    private void Awake()
    {
        GameManager.Instance.OnStartGame -= ConnectToServer;
        GameManager.Instance.OnStartGame += ConnectToServer;
    }

    // 게임 시작 시 ID 입력 및 버튼 클릭 시 서버 접속
    private void ConnectToServer()
    {
        PhotonNetwork.GameVersion = _gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        ConnectionInfoText.text = "서버에 접속 중입니다.";
    }

    public override void OnConnectedToMaster()
    {
        _connectionInfoText.text = "서버 연결에 성공하였습니다.";
        OnLogInSuccess.Invoke();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        _connectionInfoText.text = "서버 연결에 실패하였습니다.";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            _connectionInfoText.text = "룸에 접속 중입니다.";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            _connectionInfoText.text = "서버 연결에 실패하였습니다.";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        _connectionInfoText.text = "게임이 없습니다. 새로 생성합니다.";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 }); // 이거 게임모드에 맞게 변경 필요 // 게임모드를 스테이지가 아니라 로비에서 선택하도록
    }

    public override void OnJoinedRoom()
    {
        _connectionInfoText.text = "아레나가 열리고 있습니다. 상대를 기다리고 있습니다.";
        PhotonNetwork.LoadLevel("InGame");
        OnMatchSuccess.Invoke(GameModeType.Duel);
    }
    public void OnLevelWasLoaded(int level)
    {
        GameMode currentGameMode = GameManager.Instance.StageManager.CurrentGameMode;
        OnEnteringGameMode.Invoke(currentGameMode);
    }
}

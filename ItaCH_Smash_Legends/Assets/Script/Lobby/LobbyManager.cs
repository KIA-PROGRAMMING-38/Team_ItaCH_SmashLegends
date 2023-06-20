using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Util.Path;
using System;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string _gameVersion = "1";

    public TextMeshProUGUI ConnectionInfoText { get => _connectionInfoText; set => _connectionInfoText = value; } // 로딩 패널에서 현재 접속 상태를 보여줄 텍스트    
    private TextMeshProUGUI _connectionInfoText;
    public Button GameStartButton; // 게임 시작 버튼 클릭 시 매칭 시작
    private GameMode _currentGameMode; // 현재 스테이지 매니저에서 진행하고 있던 모드 초기화를 여기서 실행하게
    public Button PlayGameButton { get => _playGameButton; set => _playGameButton = value; }
    private Button _playGameButton;
    public event Action<GameModeType> OnMatchSuccess;

    // 게임 시작 시 ID 입력 이후 서버 접속
    private void Awake()
    {        
        GameManager.Instance.StartGame -= ConnectToServer;
        GameManager.Instance.StartGame += ConnectToServer;
    }
    private void SetLobbyUI()
    {
        GameObject lobbyUIPrefab = Resources.Load<GameObject>(FilePath.UIResources + "LobbyUI");
        Instantiate(lobbyUIPrefab);
    }
    private void ConnectToServer()
    {
        PhotonNetwork.GameVersion = _gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        //GameStartButton.interactable = false;
        _connectionInfoText.text = "서버에 접속 중입니다.";
    }
    public override void OnConnectedToMaster()
    {
        //GameStartButton.interactable = true;
        _connectionInfoText.text = "서버 연결에 성공하였습니다."; 
        SetLobbyUI();
        GameManager.Instance.OnLogInSuccess();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //GameStartButton.interactable = true;
        _connectionInfoText.text = "서버 연결에 실패하였습니다.";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        //GameStartButton.interactable = false;
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
        OnMatchSuccess.Invoke(GameModeType.None);
    }

}

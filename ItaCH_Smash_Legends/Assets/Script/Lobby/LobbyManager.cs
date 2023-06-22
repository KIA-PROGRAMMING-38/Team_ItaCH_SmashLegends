using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string _gameVersion = "1";

    public TextMeshProUGUI ConnectionInfoText; // 로딩 패널에서 현재 접속 상태를 보여줄 텍스트
    public Button GameStartButton; // 게임 시작 버튼 클릭 시 매칭 시작
    private GameMode _currentGameMode; // 현재 스테이지 매니저에서 진행하고 있던 모드 초기화를 여기서 실행하게
    private void Start()
    {
        //ConnectToServer();
    }

    private void ConnectToServer()
    {
        PhotonNetwork.GameVersion = _gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        //GameStartButton.interactable = false;
        ConnectionInfoText.text = "서버에 접속 중입니다.";
    }
    //public override void OnConnectedToMaster()
    //{
    //    //GameStartButton.interactable = true;
    //    ConnectionInfoText.text = "서버 연결에 성공하였습니다.";
    //}

    public override void OnDisconnected(DisconnectCause cause)
    {
        GameStartButton.interactable = true;
        ConnectionInfoText.text = "서버 연결에 실패하였습니다.";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        GameStartButton.interactable = false;
        if (PhotonNetwork.IsConnected)
        {
            ConnectionInfoText.text = "룸에 접속 중입니다.";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            ConnectionInfoText.text = "서버 연결에 실패하였습니다.";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        ConnectionInfoText.text = "게임이 없습니다. 새로 생성합니다.";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 }); // 이거 게임모드에 맞게 변경 필요 // 게임모드를 스테이지가 아니라 로비에서 선택하도록
    }

    public override void OnJoinedRoom()
    {
        ConnectionInfoText.text = "아레나가 열리고 있습니다. 상대를 기다리고 있습니다.";
        PhotonNetwork.LoadLevel("InGame"); 
    }

}

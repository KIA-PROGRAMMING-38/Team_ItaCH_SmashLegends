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

    public TextMeshProUGUI ConnectionInfoText { get => _connectionInfoText; set => _connectionInfoText = value; } // �ε� �гο��� ���� ���� ���¸� ������ �ؽ�Ʈ    
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

    // ���� ���� �� ID �Է� �� ��ư Ŭ�� �� ���� ����
    private void ConnectToServer()
    {
        PhotonNetwork.GameVersion = _gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        ConnectionInfoText.text = "������ ���� ���Դϴ�.";
    }

    public override void OnConnectedToMaster()
    {
        _connectionInfoText.text = "���� ���ῡ �����Ͽ����ϴ�.";
        OnLogInSuccess.Invoke();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        _connectionInfoText.text = "���� ���ῡ �����Ͽ����ϴ�.";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            _connectionInfoText.text = "�뿡 ���� ���Դϴ�.";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            _connectionInfoText.text = "���� ���ῡ �����Ͽ����ϴ�.";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        _connectionInfoText.text = "������ �����ϴ�. ���� �����մϴ�.";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 }); // �̰� ���Ӹ�忡 �°� ���� �ʿ� // ���Ӹ�带 ���������� �ƴ϶� �κ񿡼� �����ϵ���
    }

    public override void OnJoinedRoom()
    {
        _connectionInfoText.text = "�Ʒ����� ������ �ֽ��ϴ�. ��븦 ��ٸ��� �ֽ��ϴ�.";
        PhotonNetwork.LoadLevel("InGame");
        OnMatchSuccess.Invoke(GameModeType.Duel);
    }
    public void OnLevelWasLoaded(int level)
    {
        GameMode currentGameMode = GameManager.Instance.StageManager.CurrentGameMode;
        OnEnteringGameMode.Invoke(currentGameMode);
    }
}

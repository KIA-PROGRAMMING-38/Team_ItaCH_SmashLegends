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

    public TextMeshProUGUI ConnectionInfoText { get => _connectionInfoText; set => _connectionInfoText = value; } // �ε� �гο��� ���� ���� ���¸� ������ �ؽ�Ʈ    
    private TextMeshProUGUI _connectionInfoText;
    public Button GameStartButton; // ���� ���� ��ư Ŭ�� �� ��Ī ����
    private GameMode _currentGameMode; // ���� �������� �Ŵ������� �����ϰ� �ִ� ��� �ʱ�ȭ�� ���⼭ �����ϰ�
    public Button PlayGameButton { get => _playGameButton; set => _playGameButton = value; }
    private Button _playGameButton;
    public event Action<GameModeType> OnMatchSuccess;

    // ���� ���� �� ID �Է� ���� ���� ����
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
        _connectionInfoText.text = "������ ���� ���Դϴ�.";
    }
    public override void OnConnectedToMaster()
    {
        //GameStartButton.interactable = true;
        _connectionInfoText.text = "���� ���ῡ �����Ͽ����ϴ�."; 
        SetLobbyUI();
        GameManager.Instance.OnLogInSuccess();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //GameStartButton.interactable = true;
        _connectionInfoText.text = "���� ���ῡ �����Ͽ����ϴ�.";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        //GameStartButton.interactable = false;
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
        OnMatchSuccess.Invoke(GameModeType.None);
    }

}

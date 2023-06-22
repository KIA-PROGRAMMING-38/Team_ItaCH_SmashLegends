using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string _gameVersion = "1";

    public TextMeshProUGUI ConnectionInfoText; // �ε� �гο��� ���� ���� ���¸� ������ �ؽ�Ʈ
    public Button GameStartButton; // ���� ���� ��ư Ŭ�� �� ��Ī ����
    private GameMode _currentGameMode; // ���� �������� �Ŵ������� �����ϰ� �ִ� ��� �ʱ�ȭ�� ���⼭ �����ϰ�
    private void Start()
    {
        //ConnectToServer();
    }

    private void ConnectToServer()
    {
        PhotonNetwork.GameVersion = _gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        //GameStartButton.interactable = false;
        ConnectionInfoText.text = "������ ���� ���Դϴ�.";
    }
    //public override void OnConnectedToMaster()
    //{
    //    //GameStartButton.interactable = true;
    //    ConnectionInfoText.text = "���� ���ῡ �����Ͽ����ϴ�.";
    //}

    public override void OnDisconnected(DisconnectCause cause)
    {
        GameStartButton.interactable = true;
        ConnectionInfoText.text = "���� ���ῡ �����Ͽ����ϴ�.";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        GameStartButton.interactable = false;
        if (PhotonNetwork.IsConnected)
        {
            ConnectionInfoText.text = "�뿡 ���� ���Դϴ�.";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            ConnectionInfoText.text = "���� ���ῡ �����Ͽ����ϴ�.";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        ConnectionInfoText.text = "������ �����ϴ�. ���� �����մϴ�.";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 }); // �̰� ���Ӹ�忡 �°� ���� �ʿ� // ���Ӹ�带 ���������� �ƴ϶� �κ񿡼� �����ϵ���
    }

    public override void OnJoinedRoom()
    {
        ConnectionInfoText.text = "�Ʒ����� ������ �ֽ��ϴ�. ��븦 ��ٸ��� �ֽ��ϴ�.";
        PhotonNetwork.LoadLevel("InGame"); 
    }

}

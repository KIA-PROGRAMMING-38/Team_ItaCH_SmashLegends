using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string _gameVersion = "1";
    public TextMeshProUGUI ConnectionInfoText { get => _connectionInfoText; set => _connectionInfoText = value; } // 로딩 패널에서 현재 접속 상태를 보여줄 텍스트    
    private TextMeshProUGUI _connectionInfoText;
    public Button ExitRoomButton { get => _exitRoomButton; set => _exitRoomButton = value; }
    private Button _exitRoomButton;

    public event Action OnLogInSuccess;
    public event Action<GameModeType> OnMatchSuccess;
    public event Action<GameMode> OnEnteringGameMode;
    public event Action<int, UserData> OnUpdateUserDatas;

    private int _totalPlayerOfGameMode = 4; // 게임 모드 선택 기능 추가 시 해당 숫자 값 비우고 모드 값으로 할당    

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
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = _totalPlayerOfGameMode }); // 게임모드에 맞게 변경 필요 // 게임모드를 스테이지가 아니라 로비에서 선택하도록
    }

    public override void OnJoinedRoom()
    {
        _connectionInfoText.text = "아레나가 열리고 있습니다. 상대를 기다리고 있습니다.";
        ResisterUserLocalData();
        MatchWithBot();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 상대 데이터 받아오는 부분        
    }
    private void EnterInGameScene()
    {
        PhotonNetwork.LoadLevel("InGame");
        OnMatchSuccess.Invoke(GameModeType.Duel);
    }

    public void OnLevelWasLoaded(int level)
    {
        GameMode currentGameMode = GameManager.Instance.StageManager.CurrentGameMode;
        OnEnteringGameMode.Invoke(currentGameMode);
    }


    private void ResisterUserLocalData()
    {
        int enteringOrder = GetEnteringOrder();
        UserData userLocalData = GetUserLocalData();
        userLocalData.Id = enteringOrder;
        OnUpdateUserDatas.Invoke(enteringOrder, userLocalData);
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
        UserData userLocalData = GameManager.Instance.UserManager.UserLocalData;
        return userLocalData;
    }

    private async UniTask MatchWithBot()
    {
        await UniTask.Delay(2000); // 현재 2초 동안 매칭 안 잡히면 연습장 자동 입장
        EnterInGameScene();
    }
}
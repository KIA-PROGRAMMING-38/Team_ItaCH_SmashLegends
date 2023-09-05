using Cysharp.Threading.Tasks;
using DG.Tweening;
using Photon.Pun;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class UI_LogInPopup : UIPopup
{
    enum Texts
    {
        UserNameText,
        ErrorMessageText,
        ConnectionInfoText
    }

    enum Images
    {
        BackGround,
        SpinnerImage
    }

    enum Buttons
    {
        StartButton
    }

    enum GameObjects
    {
        TitleLogo,
        InputBox,
        ConnectionInfo
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            gameObject.SetActive(false);
        }
        Init();
    }

    private void Init()
    {
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.StartButton).gameObject.BindEvent(SetName);

        GetObject((int)GameObjects.InputBox).SetActive(false);
        GetObject((int)GameObjects.ConnectionInfo).SetActive(false);
        GetText((int)Texts.ErrorMessageText).enabled = false;

        ShowOpeningAsync().Forget();

        // LobbyManager에서 서버 접속 상태 변경에 따라 UI 텍스트 변경 위한 구독        
        Managers.LobbyManager.OnConnectingtoServer -= () => SetConnectionInfo(StringLiteral.CONNECT_SERVER);
        Managers.LobbyManager.OnConnectingtoServer += () => SetConnectionInfo(StringLiteral.CONNECT_SERVER);

        Managers.LobbyManager.OnDisconnectedfromServer -= () => SetConnectionInfo(StringLiteral.CONNECTION_FAILURE);
        Managers.LobbyManager.OnDisconnectedfromServer += () => SetConnectionInfo(StringLiteral.CONNECTION_FAILURE);     
    }

    public async UniTask ShowOpeningAsync()
    {
        float fadeInDuration = 3f;
        GetImage((int)Images.BackGround).DOColor(Color.white, fadeInDuration);

        float firstTargetScale = 1.5f;
        float endScale = 1f;
        float getLargerDuration = 0.8f;
        float getSmallerDuration = 0.7f;

        RectTransform titleLogo = GetObject((int)GameObjects.TitleLogo).GetComponent<RectTransform>();
        titleLogo.DOScale(firstTargetScale, getLargerDuration)
            .OnComplete(() => titleLogo.DOScale(endScale, getSmallerDuration));

        await UniTask.Delay(2000);

        GetObject((int)GameObjects.InputBox).SetActive(true);
    }

    private const string INPUT_PATTERN = @"^[a-zA-Z가-힣]{2,8}";

    public void SetName()
    {
        Managers.SoundManager.Play(SoundType.SFX, StringLiteral.SFX_BUTTON);

        string userInput = GetText((int)Texts.UserNameText).text;

        if (Regex.IsMatch(userInput, INPUT_PATTERN))
        {
            Managers.LobbyManager.UserLocalData.Name = userInput;

            GetObject((int)GameObjects.InputBox).SetActive(false);
            GetObject((int)GameObjects.ConnectionInfo).SetActive(true);
            
            GetImage((int)Images.SpinnerImage).GetComponent<RectTransform>()
                .DORotate(Vector3.back * 360, 0.3f, RotateMode.FastBeyond360).SetLoops(-1);

            Managers.LobbyManager.ConnectToServer();
        }
        else
        {
            GetText((int)Texts.ErrorMessageText).enabled = true;
        }
    }

    private void SetConnectionInfo(string text) => GetText((int)Texts.ConnectionInfoText).text = text;
}

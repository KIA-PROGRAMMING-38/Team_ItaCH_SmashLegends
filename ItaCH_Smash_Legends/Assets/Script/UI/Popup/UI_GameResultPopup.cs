using Cysharp.Threading.Tasks;
using Photon.Pun;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_GameResultPopup : UIPopup
{
    enum Texts
    {
        TopDecoText
    }

    enum Images
    {
        TopDecoImage
    }

    enum Buttons
    {
        ExitButton
    }

    enum GameObjects
    {
        GameResultUserProfiles
    }
    enum GameResultUserProfiles
    {
        BlueTeamUserProfile,
        RedTeamUserProfile
    }

    private GameObject _gameResultLegendModelSpace;
    private Transform[] _legendModelSpawnPoints;
    public override void Init()
    {
        base.Init();

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));
        Bind<UI_GameResultSubItem>(typeof(GameResultUserProfiles));

        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(() => OnCloseButton().Forget());
    }

    private List<UI_GameResultSubItem> _profiles = new List<UI_GameResultSubItem>();

    public void SetInfo(TeamType winnerTeam)
    {
        SetGameResultModelSpace();
        SetTopDeco(winnerTeam);

        PopulateProfile();
        RefreshProfileItems(winnerTeam);
    }

    public void SetTopDeco(TeamType winnerTeam)
    {
        TeamType localUserTeam = Managers.LobbyManager.UserLocalData.TeamType;
        string topDecoPath = Path.Combine(StringLiteral.UI_SPRITE_FOLDER, StringLiteral.UI_GAME_RESULT_POPUP, $"{StringLiteral.UI_GAME_RESULT_POPUP}{StringLiteral.TOP_DECO_SPRITE}{localUserTeam.ToString()}");
        GetImage((int)Images.TopDecoImage).sprite = Managers.ResourceManager.Load<Sprite>(topDecoPath);

        string topDecoText = default;

        if (winnerTeam == TeamType.None)
        {
            topDecoText = "무승부";
        }
        else if (localUserTeam == winnerTeam)
        {
            topDecoText = "승리";
        }
        else
        {
            topDecoText = "패배";
        }

        GetText((int)Texts.TopDecoText).text = topDecoText;
    }

    public void SetGameResultModelSpace()
    {
        _gameResultLegendModelSpace = Managers.ResourceManager.Instantiate("UI/SubItem/UI_GameResultLegendModelSpace", gameObject.transform.parent);
        this.GetOrAddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        this.GetOrAddComponent<Canvas>().worldCamera = Utils.FindChild<Camera>(_gameResultLegendModelSpace, "ResultCamera");
        _legendModelSpawnPoints = _gameResultLegendModelSpace.GetComponentsInChildren<Transform>();
    }

    private void PopulateProfile()
    {
        _profiles.Clear();

        GameObject parentObject = GetObject((int)GameObjects.GameResultUserProfiles);

        foreach (Transform child in parentObject.transform)
        {
            Managers.ResourceManager.Destroy(child.gameObject);
        }

        int maxTeamCount = Managers.StageManager.CurrentGameMode.MaxTeamCount;

        for (int team = (int)TeamType.Blue; team <= maxTeamCount; ++team)
        {
            UserData userData = Managers.StageManager.CurrentGameMode.Teams[team].Members[0];
            CreateProfileItem(userData, parentObject);
        }
    }

    public void CreateProfileItem(UserData user, GameObject parentObject)
    {
        UI_GameResultSubItem profileItem = Managers.UIManager.MakeSubItem<UI_GameResultSubItem>(parentObject.transform);
        profileItem.SetInfo(user, _legendModelSpawnPoints[(int)user.TeamType]);
        _profiles.Add(profileItem);
    }

    private void RefreshProfileItems(TeamType winnerTeam)
    {
        foreach (UI_GameResultSubItem profile in _profiles)
        {
            profile.RefreshSubItem(winnerTeam);
        }
    }

    private async UniTask OnCloseButton()
    {
        PhotonNetwork.LoadLevel(StringLiteral.LOBBY);

        await UniTask.WaitUntil(() => PhotonNetwork.LevelLoadingProgress == 1);

        Managers.UIManager.ClosePopupUI();
        Destroy(_gameResultLegendModelSpace);
        Managers.UIManager.ShowPopupUI<UI_LobbyPopup>();
        PhotonNetwork.LeaveRoom();
    }
}
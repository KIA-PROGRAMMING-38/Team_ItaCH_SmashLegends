using Photon.Pun;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

    public override void Init()
    {
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));
        //Bind<UI_GameResultSubItem>(typeof(GameResultUserProfiles));

        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnCloseButton);
    }

    private List<UI_GameResultSubItem> _profiles = new List<UI_GameResultSubItem>();

    public void SetInfo(TeamType winnerTeam)
    {        
        SetTopDeco(winnerTeam);        
        // PopulateProfile();
        // RefreshSubItems();
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
            //CreateProfileItem(team, parentObject);
        }
    }

    public void CreateProfileItem(int team, GameObject parentObject)
    {
        UI_GameResultSubItem profileItem = Managers.UIManager.MakeSubItem<UI_GameResultSubItem>(parentObject.transform);

        profileItem.SetInfo(team);
        _profiles.Add(profileItem);
    }

    private void RefreshSubItems()
    {
        foreach (UI_GameResultSubItem profile in _profiles)
        {
            profile.RefreshSubItem();
        }
    }

    private void OnCloseButton()
    {        
        PhotonNetwork.LoadLevel(StringLiteral.LOBBY);
        Managers.UIManager.ClosePopupUI(this);
        // Managers.UIManager.ShowPopupUI<UI_LoobyPopup>(); // To Do : UI_LobbyPopuup 구성시 또는 기존 LobbyUI 활성화
    }
}
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MatchingPopup : UIPopup
{
    enum Texts
    {
        MatchingInfoText
    }

    enum Images
    {
        CurrentGameModeIconImage,
        MatchingImage,
        MatchingCompleteImage,
        RandomLegendIconImage
    }

    enum Buttons
    {
        CloseButton
    }

    enum GameObjects
    {
        MatchingPopupSubItem
    }

    private string _currentMatchingInfo;
    private List<UI_MatchingPopupSubItem> _enteredUserboxes = new List<UI_MatchingPopupSubItem>();
    [SerializeField] private List<Sprite> _randomLegendIcon;
    private Tween _rotateIcon;
    public override void Init()
    {
        base.Init();

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnCloseButton);

        Managers.LobbyManager.OnJoiningRoom -= () => SetMatchingInfo(StringLiteral.ENTER_ROOM);
        Managers.LobbyManager.OnJoiningRoom += () => SetMatchingInfo(StringLiteral.ENTER_ROOM);

        Managers.LobbyManager.OnCreatingRoom -= () => SetMatchingInfo(StringLiteral.CREATE_ROOM);
        Managers.LobbyManager.OnCreatingRoom += () => SetMatchingInfo(StringLiteral.CREATE_ROOM);

        Managers.LobbyManager.OnWaitingPlayer -= () => SetMatchingInfo(StringLiteral.WAIT_PLAYER);
        Managers.LobbyManager.OnWaitingPlayer += () => SetMatchingInfo(StringLiteral.WAIT_PLAYER);

        Managers.LobbyManager.OnMatchingSuccess -= () => GetImage((int)Images.MatchingCompleteImage).enabled = true;
        Managers.LobbyManager.OnMatchingSuccess += () => GetImage((int)Images.MatchingCompleteImage).enabled = true;

        GetImage((int)Images.MatchingCompleteImage).enabled = false;
        RotateRandomLegendIconImage().Forget();

        PopulateSubItem();
    }

    private void SetMatchingInfo(string matchingInfo)
    {
        if (_currentMatchingInfo == matchingInfo)
        {
            return;
        }

        _currentMatchingInfo = matchingInfo;
        RefreshUI();
    }

    private void PopulateSubItem()
    {
        _enteredUserboxes.Clear();

        GameObject parentObject = GetObject((int)GameObjects.MatchingPopupSubItem);

        foreach (Transform child in parentObject.transform)
        {
            Managers.ResourceManager.Destroy(child.gameObject);
        }

        int maxTeamCount = Managers.StageManager.CurrentGameMode.MaxTeamCount;

        for (int team = (int)TeamType.Blue; team <= maxTeamCount; ++team)
        {
            CreateEnteredUserBoxes(team, parentObject);
        }
    }

    private void CreateEnteredUserBoxes(int team, GameObject parentObject)
    {
        UI_MatchingPopupSubItem enteredUserBoxes = Managers.UIManager.MakeSubItem<UI_MatchingPopupSubItem>(parentObject.transform);

        enteredUserBoxes.SetInfo(team);
        _enteredUserboxes.Add(enteredUserBoxes);
    }

    private async UniTask RotateRandomLegendIconImage()
    {
        const int ROTATION_SPEED = 1;
        const int END_ROTATION_ANGLE = 370;

        RectTransform randomLegendIcon = GetImage((int)Images.RandomLegendIconImage).GetComponent<RectTransform>();

        while (this.gameObject.activeSelf)
        {
            randomLegendIcon.rotation = Quaternion.Euler(Vector3.zero);
            GetImage((int)Images.RandomLegendIconImage).sprite = _randomLegendIcon[Random.Range(0, _randomLegendIcon.Count)];

            _rotateIcon = randomLegendIcon.DORotate(Vector3.back * END_ROTATION_ANGLE, ROTATION_SPEED, RotateMode.FastBeyond360);

            await _rotateIcon.AsyncWaitForCompletion();
        }
    }

    public void RefreshUI()
    {
        GetText((int)Texts.MatchingInfoText).text = _currentMatchingInfo;

        foreach (UI_MatchingPopupSubItem subItem in _enteredUserboxes)
        {
            subItem.RefreshUI();
        }
    }

    public override void ClosePopupUI()
    {
        _rotateIcon.Kill();
        base.ClosePopupUI();
    }

    private void OnCloseButton()
    {
        Managers.UIManager.ClosePopupUI(this);

        // TO DO : Photon 룸 접속 해제
        // TO DO : 로비 화면으로 돌아갈 때 레전드 보이스 재생 여부 체크
    }

    private void OnDestroy()
    {
        _enteredUserboxes.Clear();

        Managers.LobbyManager.OnJoiningRoom -= () => SetMatchingInfo(StringLiteral.ENTER_ROOM);
        Managers.LobbyManager.OnCreatingRoom -= () => SetMatchingInfo(StringLiteral.CREATE_ROOM);
        Managers.LobbyManager.OnWaitingPlayer -= () => SetMatchingInfo(StringLiteral.WAIT_PLAYER);
        Managers.LobbyManager.OnMatchingSuccess -= () => GetImage((int)Images.MatchingCompleteImage).enabled = true;
    }
}

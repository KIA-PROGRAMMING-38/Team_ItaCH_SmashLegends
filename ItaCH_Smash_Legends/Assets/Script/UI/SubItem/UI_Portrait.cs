using UnityEngine;

public class UI_Portrait : UIBase
{
    enum Images
    {
        LegendFaceImage,
        RespawnTimeSpinner
    }

    enum Texts
    {
        UserNameText,
        RespawnTimeText
    }

    enum GameObjects
    {
        UserName,
        LegendFaceImage,
        RespawnTime
    }

    UserData _userData;

    public override void Init()
    {
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindObject(typeof(GameObjects));

        GetObject((int)GameObjects.RespawnTime).gameObject.SetActive(false);
    }

    public void SetInfo(UserData userData, bool isInitialSet = false)
    {
        _userData = userData;

        if (isInitialSet)
        {
            Init();
            RefreshUserName();
            RefreshLegendFaceImage();
        }

        RefreshRespawnTimer();
    }

    private void RefreshUserName()
    {
        GetText((int)Texts.UserNameText).text = _userData.Name;
        if (_userData.TeamType == TeamType.Red)
        {
            Utils.ReverseAxisY(GetText((int)Texts.UserNameText).GetComponent<RectTransform>());
        }
    }

    private void RefreshLegendFaceImage()
    {
        GetImage((int)Images.LegendFaceImage).sprite = Managers.ResourceManager.GetLegendFaceImage(_userData.SelectedLegend);
    }

    private void RefreshRespawnTimer()
    {
        // To Do : Spinner spin & TimerText reduce
    }
}

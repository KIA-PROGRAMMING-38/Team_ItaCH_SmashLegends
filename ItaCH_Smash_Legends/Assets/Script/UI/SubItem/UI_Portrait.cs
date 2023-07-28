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

    public override void Init()
    {
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindObject(typeof(GameObjects));

        RefreshUI();

        GetObject((int)GameObjects.RespawnTime).gameObject.SetActive(false);
    }

    private void RefreshUI()
    {
        RefreshUserName();
        RefreshLegendFaceImage();
    }

    private void RefreshUserName()
    {
        // To Do : Bind With UserData
    }

    private void RefreshLegendFaceImage()
    {
        // To Do : Bind With UserData
    }

    private void RefreshRespawnTimer()
    {
        // To Do : Spinner spin & TimerText reduce
    }
}

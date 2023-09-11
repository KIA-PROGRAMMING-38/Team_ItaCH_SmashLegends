using Cysharp.Threading.Tasks;
using System.Threading;
using Unity.VisualScripting;
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

    public void SetInfo(UserData userData)
    {
        _userData = userData;
        SetLegendFaceImage();
        SetUserName();

        userData.OwnedLegend.OnDie -= RefreshRespawnTimer;
        userData.OwnedLegend.OnDie += RefreshRespawnTimer;

        if (userData.TeamType == TeamType.Red)
        {
            GetText((int)Texts.UserNameText).GetComponent<RectTransform>().FlipY();
            GetObject((int)GameObjects.RespawnTime).GetComponent<RectTransform>().FlipY();
        }
    }

    private void SetUserName()
    {
        GetText((int)Texts.UserNameText).text = _userData.Name;
    }

    private void SetLegendFaceImage()
    {
        GetImage((int)Images.LegendFaceImage).sprite = Managers.ResourceManager.GetLegendSprite(StringLiteral.UI_DUEL_MODE_POPUP, _userData.SelectedLegend);
    }

    private async void RefreshRespawnTimer()
    {
        GetObject((int)GameObjects.RespawnTime).SetActive(true);
        GetImage((int)Images.LegendFaceImage).color = Color.gray;

        float respawnTime = Managers.StageManager.CurrentGameMode.ModeDefaultRespawnTime;
        CancellationTokenSource rotateImage = new CancellationTokenSource();

        GetImage((int)Images.RespawnTimeSpinner).rectTransform.RotateRectTransformAsync(Vector3.back, 360, rotateImage.Token).Forget();

        await RefreshPlayerRespawnTimerTextTask(respawnTime, rotateImage);

        GetObject((int)GameObjects.RespawnTime).SetActive(false);
        GetImage((int)Images.LegendFaceImage).color = Color.white;
    }

    private const int ONE_SECOND = 1000;

    private async UniTask RefreshPlayerRespawnTimerTextTask(float respawnTime, CancellationTokenSource rotateImage)
    {
        float elapsedTime = 0;
        while (elapsedTime < respawnTime)
        {
            GetText((int)Texts.RespawnTimeText).text = $"{respawnTime - elapsedTime}";
            await UniTask.Delay(ONE_SECOND);
            ++elapsedTime;
        }

        rotateImage.Cancel();
    }
}

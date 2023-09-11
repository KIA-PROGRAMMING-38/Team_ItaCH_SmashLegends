using UnityEngine;

public class UI_EnteredUserBox : UIBase
{
    enum Images
    {
        UI_EnteredUserBox,
        EnteredUserHighlightImage
    }

    public override void Init()
    {
        BindImage(typeof(Images));
        GetImage((int)Images.EnteredUserHighlightImage).enabled = false;
    }

    private const int DEFAULT_POSITION_X = -300;
    private const int POSITION_X_INTERVAL = -180;
    public void SetInfo(int id)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(DEFAULT_POSITION_X + id * POSITION_X_INTERVAL, 0, 0);
    }

    public void RefreshUI(bool isEntered = false)
    {
        if (isEntered)
        {
            GetImage((int)Images.UI_EnteredUserBox).color = Define.USER_ENTERED_COLOR;
            GetImage((int)Images.EnteredUserHighlightImage).enabled = true;
        }

        else
        {
            GetImage((int)Images.UI_EnteredUserBox).color = Define.DEFAULT_ENTERED_USER_BOX_COLOR;
            GetImage((int)Images.EnteredUserHighlightImage).enabled = false;
        }
    }
}

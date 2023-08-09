using UnityEngine;

public class UI_ScoreSetSubItem : UIBase
{
    private const int POSITION_X_INTERVAL = -66;
    private const int POSITION_Y_INTERVAL = 10;

    enum GameObjects
    {
        ScoreFill
    }

    public override void Init()
    {
        BindObject(typeof(GameObjects));
    }

    public void SetInfo(int index)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3((index + 1) * POSITION_X_INTERVAL, POSITION_Y_INTERVAL, 0); // index 0 제외        
        GetObject((int)GameObjects.ScoreFill).gameObject.SetActive(false);
    }

    public void ActivateScoreSetSubItem()
    {
        GetObject((int)GameObjects.ScoreFill).gameObject.SetActive(true);
    }
}

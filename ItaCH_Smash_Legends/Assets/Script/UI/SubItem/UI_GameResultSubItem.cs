using UnityEngine;
public class UI_GameResultSubItem : UIBase
{
    enum Texts
    { 
        UserNameText
    }

    enum GameObjects
    {
        UserSelectedLegendModel
    }

    public override void Init()
    {
        BindText(typeof(Texts));
        BindObject(typeof(GameObjects));
    }

    public void SetInfo(int team)
    {        
        this.GetComponent<RectTransform>().anchoredPosition = Define.GAME_RESULT_USER_PROFILE_POSITIONS[team];
        // 모델 지정
    }
    
    public void RefreshSubItem()
    {

    }
}
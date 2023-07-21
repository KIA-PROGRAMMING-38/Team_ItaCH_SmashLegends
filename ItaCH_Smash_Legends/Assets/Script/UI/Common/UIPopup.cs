public class UIPopup : UIBase
{
    public override void Init()
    {
        Managers.UIManager.SetCanvas(gameObject, true);
    }

    public virtual void ClosePopupUI()
    {
        Managers.UIManager.ClosePopupUI(this);
    }
}

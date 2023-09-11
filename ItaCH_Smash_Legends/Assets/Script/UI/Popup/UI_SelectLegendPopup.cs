using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SelectLegendPopup : UIPopup
{
    enum Buttons
    {
        CloseButton
    }

    enum GameObjects
    {
        SelectLegendButtons
    }

    private List<UI_SelectLegendPopupSubItem> _selectLegendButtons = new List<UI_SelectLegendPopupSubItem>();

    public override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(ClosePopupUI);

        PopulateSelectLegendButtons();
    }

    private void PopulateSelectLegendButtons()
    {
        _selectLegendButtons.Clear();

        GameObject parentObject = GetObject((int)GameObjects.SelectLegendButtons);

        foreach (Transform child in parentObject.transform)
        {
            Managers.ResourceManager.Destroy(child.gameObject);
        }

        for (int id = 1; id < (int)LegendType.MaxCount; ++id)
        {
            CreateSelectLegendButtons(id, parentObject);
        }
    }

    private void CreateSelectLegendButtons(int id, GameObject parentObject)
    {
        UI_SelectLegendPopupSubItem selectLegendButton = Managers.UIManager.MakeSubItem<UI_SelectLegendPopupSubItem>(parentObject.transform);

        selectLegendButton.SetInfo(id);
        _selectLegendButtons.Add(selectLegendButton);
    }

    public void RefreshUI()
    {
        foreach (UI_SelectLegendPopupSubItem buttons in _selectLegendButtons)
        {
            buttons.RefreshUI();
        }
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
        Managers.UIManager.FindPopup<UI_LobbyPopup>().RefreshUI();
    }
}

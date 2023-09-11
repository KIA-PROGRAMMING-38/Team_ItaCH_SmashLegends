using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MatchingPopupSubItem : UIBase
{    
    private List<UI_EnteredUserBox> _enteredUseImageObjects = new List<UI_EnteredUserBox>();
    private TeamType _teamType;
    private int _currentMembers;
    public override void Init()
    {
        PopulateEnteredUserBox();
    }
    public void PopulateEnteredUserBox()
    {
        _enteredUseImageObjects.Clear();

        foreach (Transform child in this.transform)
        {
            Managers.ResourceManager.Destroy(child.gameObject);
        }

        int maxCount = Managers.StageManager.CurrentGameMode.MaxTeamMember;

        for (int boxIndex = 0; boxIndex < maxCount; ++boxIndex)
        {
            CreateEnteredUserBoxes(boxIndex);
        }
    }

    private void CreateEnteredUserBoxes(int id)
    {
        UI_EnteredUserBox boxItem = Managers.UIManager.MakeSubItem<UI_EnteredUserBox>(this.transform);
        boxItem.SetInfo(id);

        _enteredUseImageObjects.Add(boxItem);
    }

    public void SetInfo(int team)
    {
        _teamType = (TeamType)team;
        
        if (_teamType == TeamType.Red)
        {
            this.GetComponent<RectTransform>().FlipY();
        }
    }

    public void RefreshUI()
    {
        if (Managers.StageManager.CurrentGameMode.Teams.Count <= (int)_teamType)
        {
            return;
        }
        
        _currentMembers = Managers.StageManager.CurrentGameMode.Teams[(int)_teamType].Members.Count;

        for (int id = 0; id < Managers.StageManager.CurrentGameMode.MaxTeamMember; ++id) 
        { 
            if (id < _currentMembers) 
            {
                _enteredUseImageObjects[id].gameObject.SetActive(true);
                _enteredUseImageObjects[id].RefreshUI(isEntered: true);
            }

            else
            {
                _enteredUseImageObjects[id].gameObject.SetActive(false);
                _enteredUseImageObjects[id].RefreshUI();
            }
        }
    }

    private void OnDestroy()
    {
        _enteredUseImageObjects.Clear();
        _teamType = default;
    }
}

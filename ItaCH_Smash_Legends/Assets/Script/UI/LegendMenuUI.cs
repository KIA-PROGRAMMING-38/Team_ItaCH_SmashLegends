using System;
using UnityEngine;
using UnityEngine.UI;
using Util.Enum;

public class LegendMenuUI : MonoBehaviour, IPanel
{
    private enum LegendName
    {
        앨리스,
        후크,
        피터
    }

    [SerializeField] private Sprite[] _portraitSprites;
    [SerializeField] private GameObject _legendSelectMenuPrefab;
    private LobbyUI _lobbyUI;
    private LegendSelectUI[] _legendSelectMenu;
    private Transform _contentTransform;
    private int _numberOfLegends;

    public void InitPanelSettings(LobbyUI lobbyUI)
    {
        _lobbyUI = lobbyUI;
        _numberOfLegends = (int)CharacterType.NumOfCharacter;
        _contentTransform = GetComponentInChildren<ScrollRect>().transform.GetChild(0).GetChild(0); 
        _legendSelectMenu = new LegendSelectUI[_numberOfLegends];
        for (int i = 0; i < _numberOfLegends; ++i)
        {
            _legendSelectMenu[i] = Instantiate(_legendSelectMenuPrefab, _contentTransform).AddComponent<LegendSelectUI>();
            _legendSelectMenu[i].InitLegendSelectUI(i, _portraitSprites[i], Enum.ToObject(typeof(LegendName), i).ToString());
            
            _legendSelectMenu[i].OnSelectLegend -= RefreshFrame;
            _legendSelectMenu[i].OnSelectLegend += RefreshFrame;

            _legendSelectMenu[i].OnSelectLegend -= _lobbyUI.ChangeLobbyCharacterModel;
            _legendSelectMenu[i].OnSelectLegend += _lobbyUI.ChangeLobbyCharacterModel;
        }
        
    }

    public void RefreshFrame(int indexOfLegend)
    {
        for (int i = 0; i < _numberOfLegends; ++i)
        {
            _legendSelectMenu[i].DisableSelectFrame();
            if (i.Equals(indexOfLegend))
            {
                _legendSelectMenu[i].EnableSelectFrame();
            }
        }
    }

    public void OnDestroy()
    {
        for (int i = 0; i < _numberOfLegends; ++i)
        {
            _legendSelectMenu[i].OnSelectLegend -= RefreshFrame;
            _legendSelectMenu[i].OnSelectLegend -= _lobbyUI.ChangeLobbyCharacterModel;
        }
    }
}

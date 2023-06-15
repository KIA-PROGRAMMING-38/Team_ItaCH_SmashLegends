using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendMenuUI : MonoBehaviour
{
    [SerializeField] private Sprite[] _portraitSprites;
    [SerializeField] private GameObject _legendSelectMenuPrefab;
    private LegendSelectUI[] _legendSelectMenu;
    private Transform _contentTransform;
    private int _numberOfLegends;

    //테스트 코드. 추후 수정할 예정.
    private void Start()
    {
        InitLegendMenuUI(3);
    }
    private void InitLegendMenuUI(int numberOfLegends)
    {
        _numberOfLegends = numberOfLegends;

        _contentTransform = transform.Find("Scroll View").GetChild(0).GetChild(0);
        _legendSelectMenu = new LegendSelectUI[_numberOfLegends];
        for(int i = 0; i < _numberOfLegends; ++i)
        {
            _legendSelectMenu[i] = Instantiate(_legendSelectMenuPrefab, _contentTransform).AddComponent<LegendSelectUI>();
            _legendSelectMenu[i].InitLegendSelectUI(i, _portraitSprites[i]);
            _legendSelectMenu[i].OnSelectLegend -= RefreshFrame;
            _legendSelectMenu[i].OnSelectLegend += RefreshFrame;
        }
    }

    public void RefreshFrame(int indexOfLegend)
    {
        for(int i = 0; i < _numberOfLegends; ++i)
        {
            _legendSelectMenu[i].DisableSelectFrame();
            if(i.Equals(indexOfLegend))
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
        }
    }
}

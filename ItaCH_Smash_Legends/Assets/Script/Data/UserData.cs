using UnityEngine;

public class UserData
{
    public string Name { get; set; }
    public int ID { get; set; }
    //public Team Team { get; set; }
    //public GameModeType SelectGameMode { get; set; } // 룸 자체가 관리하도록

    private LegendType _selectedLegend;
    public LegendType SelectedLegend
    {
        get
        {
            if (_selectedLegend == LegendType.None)
            {
                _selectedLegend = (LegendType)Random.Range((int)LegendType.Alice, (int)LegendType.MaxCount);
                return _selectedLegend;
            }
            else
            {
                return _selectedLegend;
            }
        }
        set => _selectedLegend = value;
    }
    public LegendController OwnedLegend { get; set; }
}
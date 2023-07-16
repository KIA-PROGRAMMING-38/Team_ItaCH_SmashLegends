using UnityEngine;

public class UserData
{
    public string Name { get; set; }
    public int ID { get; set; }
    public Team Team { get; set; }
    public GameModeType SelectGameMode { get; set; }

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
                return _selectedLegend;
        }
        set => _selectedLegend = value;
    }

    //public ResultType GameResult { get => _gameResult; }
    //private ResultType _gameResult;

    //public void SetGameResult(TeamType winningTeam)
    //{
    //    if (TeamType.None == winningTeam) 
    //    {
    //        this._gameResult = ResultType.Draw;
    //    }
    //    else if (this.Team.Type == winningTeam)
    //    {
    //        this._gameResult = ResultType.Win;
    //    }
    //    else
    //    {
    //        this._gameResult = ResultType.Lose;
    //    }
    //}
}
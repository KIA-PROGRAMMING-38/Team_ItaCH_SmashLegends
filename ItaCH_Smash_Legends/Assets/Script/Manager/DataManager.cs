using System.IO;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using System.Linq;

internal class DataManager
{
    public Dictionary<int, LegendStatData> CharacterStats { get; private set; }
    public void Init()
    {
        CharacterStats = LoadCSV<LegendStatData>("CharacterStatData").ToDictionary(data => data.LegendID);
    }

    private IEnumerable<T> LoadCSV<T>(string name)
    {        
        string path = "data/" + name;        
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
             return csv.GetRecords<T>();
        }
    }
}
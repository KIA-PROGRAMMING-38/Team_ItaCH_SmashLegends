using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class DataManager
{
    public Dictionary<int, LegendStatData> LegendStats { get; private set; }
    public void Init()
    {
        LegendStats = LoadCSV<int, LegendStatData>(StringLiteral.LEGEND_STAT_DATA_PATH, data => data.LegendID);
    }

    private List<T> LoadCSV<T>(string path)
    {
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csv.GetRecords<T>().ToList();
        }
    }

    private Dictionary<Key, Item> LoadCSV<Key, Item>(string path, Func<Item, Key> keySelector)
    {
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csv.GetRecords<Item>().ToDictionary(keySelector);
        }
    }
}
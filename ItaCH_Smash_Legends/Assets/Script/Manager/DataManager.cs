using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

internal class DataManager
{
    public Dictionary<int, LegendStatData> LegendStats { get; private set; }
    public void Init()
    {
        LegendStats = LoadCSV<int, LegendStatData>("LegendStatData", data => data.LegendID);
    }

    private List<T> LoadCSV<T>(string name)
    {
        string path = "data/" + name;
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csv.GetRecords<T>().ToList();
        }
    }

    private Dictionary<Key, Item> LoadCSV<Key, Item>(string name, Func<Item, Key> keySelector)
    {
        string path = "data/" + name;
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csv.GetRecords<Item>().ToDictionary(keySelector);
        }
    }
}
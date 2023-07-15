using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceManager
{
    public Dictionary<string, Sprite> Sprites { get; private set; }
    public Dictionary<string, LegendController> Legends { get; private set; }

    public void Init()
    {
        Sprites = new Dictionary<string, Sprite>();
        Legends = new Dictionary<string, LegendController>();
    }

    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(Sprite))
        {
            if (Sprites.TryGetValue(path, out Sprite sprite))
            {
                return sprite as T;
            }

            Sprite sp = Resources.Load<Sprite>(path);
            Sprites.Add(path, sp);
            return sp as T;
        }

        else if (typeof(T) == typeof(LegendController))
        {
            if (Legends.TryGetValue(path, out LegendController characterStatus))
            {
                return characterStatus as T;
            }

            LegendController legend = Resources.Load<LegendController>(path);
            Legends.Add(path, legend);
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.LogError($"Failed to load prefab : {path}");
            return null;
        }

        return Instantiate(prefab, parent);
    }

    public GameObject Instantiate(GameObject prefab, Transform parent = null)
    {
        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        Object.Destroy(go);
    }

    public LegendController GetLegendPrefab(LegendType legend)
    {
        string legendName = legend.ToString();
        string legendPrefabPath = Path.Combine(StringLiteral.PREFAB_FOLDER, legendName, $"{legendName}{StringLiteral.SUFFIX_INGAME}", $"{legendName}{StringLiteral.SUFFIX_INGAME}");
        return Load<LegendController>(legendPrefabPath);
    }
}
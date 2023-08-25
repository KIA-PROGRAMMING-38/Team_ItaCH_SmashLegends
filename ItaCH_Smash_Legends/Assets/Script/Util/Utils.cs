using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Utils
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
        {
            return null;
        }

        if (recursive == false)
        {
            Transform transform = go.transform.Find(name);

            return transform.GetComponent<T>();
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                {
                    return component;
                }
            }
        }

        return null;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform targetTransform = FindChild<Transform>(go, name, recursive);
        if (targetTransform == null)
        {
            return null;
        }
        return targetTransform.gameObject;
    }

    // To Do : UI 개선 이후 사라질 불분명한 이름의 불필요한 method들
    public static float AbsFloat(float someFloat)
    {
        if (someFloat > 0)
        {
            return someFloat;
        }
        else
        {
            return -someFloat;
        }
    }

    public static bool IsPositive(float someFloat)
    {
        if (someFloat > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static async UniTask Rotate(CancellationToken cancellationToken, RectTransform rectTransform, Vector3 direction, float turnSpeed)
    {
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            rectTransform.Rotate(direction * (turnSpeed * Time.fixedDeltaTime));
            await UniTask.DelayFrame(1);
        }
    }
}

using Cysharp.Threading.Tasks;
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

    public static float CalculateAbsolute(float someFloat)
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

    public static async UniTask ChangeFillAmountGradually(float targetValue, float targetTime, Image image)
    {
        float elapsedTime = 0;
        float startValue = image.fillAmount;
        while (elapsedTime <= targetTime)
        {
            float currentValueRatio = Mathf.Clamp01(elapsedTime / targetTime);
            image.fillAmount = Mathf.Lerp(startValue, targetValue, currentValueRatio);
            elapsedTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }
        image.fillAmount = targetValue;
    }
}
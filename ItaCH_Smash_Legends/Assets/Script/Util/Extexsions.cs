using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

public static class Extensions
{
    public static void FlipY(this RectTransform rectTransform, Space RelativeTo = Space.Self)
    {
        rectTransform.Rotate(0, 180, 0, RelativeTo);
    }

    public static void BindEvent(this GameObject go, Action action, UIEventType type = UIEventType.Click)
    {
        UIBase.BindEvent(go, action, type);
    }

    public static async UniTask ChangeFillAmountGradually(this Image image, float targetValue, float targetTime)
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
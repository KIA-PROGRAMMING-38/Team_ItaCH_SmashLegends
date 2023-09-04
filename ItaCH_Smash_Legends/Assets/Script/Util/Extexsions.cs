using Cysharp.Threading.Tasks;
using System;
using System.Threading;
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
        float elapsedTime = 0f;
        float startValue = image.fillAmount;

        while (elapsedTime < targetTime)
        {
            elapsedTime += Time.deltaTime;
            image.fillAmount = Mathf.Lerp(startValue, targetValue, elapsedTime / targetTime);

            await UniTask.DelayFrame(1);
        }

        image.fillAmount = targetValue;
    }

    public static async UniTask RotateRectTransformAsync(this RectTransform ui, Vector3 direction, float turnSpeed, CancellationToken cancellationToken)
    {
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ui.Rotate(direction * (turnSpeed * Time.fixedDeltaTime));
            await UniTask.DelayFrame(1);
        }
    }
}
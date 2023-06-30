using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Util.Method
{
    public static class Method
    {
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

        public static async UniTask ChangeFillAmountGradually(float startValue, float targetValue, Image image)
        {
            float elapsedTime = 0;
            float changingTime = 1f;
            while (elapsedTime <= changingTime)
            {
                float currentValueRatio = Mathf.Clamp01(elapsedTime / changingTime);
                image.fillAmount = Mathf.Lerp(startValue, targetValue, currentValueRatio);
                elapsedTime += Time.deltaTime;
                await UniTask.DelayFrame(1);
            }
        }
    }
}
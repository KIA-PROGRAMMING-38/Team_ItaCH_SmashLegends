using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

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
    }
}
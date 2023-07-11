using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Util.Method;

public class HealthBar : MonoBehaviour
{
    private Image _filling;
    private Image _fillingBackground;
    private bool _isSetHealthPointCalledBefore;
    private CancellationTokenSource _cancellationTokenSource;

    public void InitHealthBarSettings()
    {
        _fillingBackground = transform.GetChild(0).GetComponent<Image>();
        _filling = transform.GetChild(1).GetComponent<Image>();
        _isSetHealthPointCalledBefore = false;
    }

    public async void SetHealthPoint(int healthPoint, int healthPointPercent)
    {
        if (_isSetHealthPointCalledBefore)
        {
            _cancellationTokenSource?.Cancel();
        }

        // 단순히 피를 회복하여 풀피가 되는 상황을 제외하기 위해 500으로 설정.
        if (healthPointPercent >= 500f)
        {
            await Method.ChangeFillAmountGradually(1, 0.5f, _filling);
            healthPointPercent = 100;
            _fillingBackground.fillAmount = 1;
        }
        float healthPointRatio = healthPointPercent * 0.01f;
        _filling.fillAmount = healthPointRatio;

        _cancellationTokenSource = new CancellationTokenSource();
        _isSetHealthPointCalledBefore = true;

        var WaitTask = WaitForDamage(healthPointRatio, _cancellationTokenSource.Token);
        WaitTask.Forget();
    }

    private async UniTask WaitForDamage(float healthPointRatio, CancellationToken cancellationToken)
    {
        await UniTask.Delay(1000);
        cancellationToken.ThrowIfCancellationRequested();
        await Method.ChangeFillAmountGradually(healthPointRatio, 0.5f, _fillingBackground);
    }
}

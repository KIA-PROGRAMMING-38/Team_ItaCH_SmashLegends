using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

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

        // TO DO : legacy 수정
        if (healthPointPercent >= 500f)
        {
            await _filling.ChangeFillAmountGradually(1, 0.5f);
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
        await _fillingBackground.ChangeFillAmountGradually(healthPointRatio, 0.5f);
    }
}

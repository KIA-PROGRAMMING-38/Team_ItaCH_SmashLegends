using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.UI;

public class UI_HpBar : UIBase
{
    enum Images
    {
        DamageBuffer,
        HpBarFill
    }

    private const float MAX_FILL_AMOUNT = 1f;
    private const int DAMAGE_BUFFER_DELAY_TIME = 1000;
    private const float BUFFER_TIME = 0.5f;

    private Image _damageBufferImage;
    private Image _hpBarFillImage;

    private bool _isOnRefreshing;
    private CancellationTokenSource _cancellationTokenSource;

    public void Init()
    {
        _damageBufferImage = GetImage((int)Images.DamageBuffer);
        _hpBarFillImage = GetImage((int)Images.HpBarFill);
        _isOnRefreshing = false;
        
        BindImage(typeof(Images));
        RefreshUI(MAX_FILL_AMOUNT);
    }

    private void RefreshUI(float hpRatio)
    {
        if (_isOnRefreshing) 
        { 
            _cancellationTokenSource?.Cancel();
        }

        _isOnRefreshing = true;
        _cancellationTokenSource = new CancellationTokenSource();

        RefreshHpBarFill(hpRatio);
        RefreshDamageBufferTask(hpRatio, _cancellationTokenSource.Token).Forget();
    }

    private async void RefreshHpBarFill(float hpRatio)
    {
        if (hpRatio == MAX_FILL_AMOUNT)
        {
            await Utils.ChangeFillAmountGradually(MAX_FILL_AMOUNT, BUFFER_TIME, _hpBarFillImage);
        }
        _hpBarFillImage.fillAmount = hpRatio;
    }

    private async UniTask RefreshDamageBufferTask(float hpRatio, CancellationToken cancellationToken)
    {        
        await UniTask.Delay(DAMAGE_BUFFER_DELAY_TIME);
        cancellationToken.ThrowIfCancellationRequested();
        await Utils.ChangeFillAmountGradually(hpRatio, BUFFER_TIME, _damageBufferImage);
    }
}
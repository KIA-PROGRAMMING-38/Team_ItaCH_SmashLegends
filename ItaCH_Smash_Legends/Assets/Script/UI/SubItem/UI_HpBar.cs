using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
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

    private bool _isOnRefreshing;
    private CancellationTokenSource _cancellationTokenSource;

    public override void Init()
    {
        BindImage(typeof(Images));

        _isOnRefreshing = false;

        RefreshUI(MAX_FILL_AMOUNT);
    }

    public void SetInfo(UserData user)
    {
        GetImage((int)Images.DamageBuffer).color = Define.DAMAGE_BUFFER_COLORS[(int)user.TeamType];
        GetImage((int)Images.HpBarFill).color = Define.UI_PORTRAIT_COLORS[(int)user.TeamType];

        user.OwnedLegend.OnHpChanged -= RefreshUI;
        user.OwnedLegend.OnHpChanged += RefreshUI;
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
            await GetImage((int)Images.HpBarFill).ChangeFillAmountGradually(MAX_FILL_AMOUNT, BUFFER_TIME);
            // TO DO : 피격 판정 로직 적용과 함께 오류 수정
        }
        GetImage((int)Images.HpBarFill).fillAmount = hpRatio;
    }

    private async UniTask RefreshDamageBufferTask(float hpRatio, CancellationToken cancellationToken)
    {
        await UniTask.Delay(DAMAGE_BUFFER_DELAY_TIME);
        cancellationToken.ThrowIfCancellationRequested();
        await GetImage((int)Images.DamageBuffer).ChangeFillAmountGradually(hpRatio, BUFFER_TIME);
        _isOnRefreshing = false;
    }
}
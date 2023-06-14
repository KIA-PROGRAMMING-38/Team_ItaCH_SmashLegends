using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class MatchBox : MonoBehaviour
{
    private Color _initialColor;
    private Color _matchedColor;
    private Image _box;
    private Image _glow;
    private float _glowAmount;
    private CancellationTokenSource _cancellationTokenSource;

    public void InitMatchBoxSettings()
    {
        _box = GetComponent<Image>();
        _initialColor = _box.color;
        _matchedColor = Color.white;
        _glowAmount = 0f;
        _glow = transform.GetChild(0).GetComponent<Image>();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    private async UniTask GlowBox(CancellationToken cancellationToken)
    {
        bool isGlowAlphaReachedMax = false;
        bool isGlowAlphaReachedMin = true;

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (_glow.color.a <= 0)
            {
                isGlowAlphaReachedMax = false;
                isGlowAlphaReachedMin = true;
            }
            else if(_glow.color.a >= 1)
            {
                isGlowAlphaReachedMin = false;
                isGlowAlphaReachedMax = true;
            }

            if(isGlowAlphaReachedMax)
            {
                _glowAmount -= Time.deltaTime;
            }
            else if(isGlowAlphaReachedMin)
            {
                _glowAmount += Time.deltaTime;
            }

            _glow.color = new Color(1, 1, 1, _glowAmount);
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }

    public void EndBoxGlow()
    {
        _cancellationTokenSource?.Cancel();
        _glow.color = Color.clear;
        _box.color = _initialColor;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public void StartBoxGlow()
    {
        _glow.color = _matchedColor;
        _box.color = _matchedColor;

        GlowBox(_cancellationTokenSource.Token).Forget();
    }
}

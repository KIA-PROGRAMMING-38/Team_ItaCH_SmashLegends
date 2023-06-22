using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LoadingImage : MonoBehaviour
{
    private CancellationTokenSource _cancellationTokenSource;
    private CancellationToken _cancellationToken;
    private RectTransform _rectTransform;
    private Vector3 _clockWiseDirection = Vector3.back;
    private float _turnSpeed = 360;

    public void StartRotation()
    {
        _rectTransform = GetComponent<RectTransform>();
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;
        Rotate(_cancellationToken).Forget();
    }

    private async UniTask Rotate(CancellationToken cancellationToken)
    {
        while(true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _rectTransform.Rotate(_clockWiseDirection * (_turnSpeed * Time.fixedDeltaTime));
            await UniTask.DelayFrame(1);
        }
    }

    private void OnDestroy()
    {
        _cancellationTokenSource?.Cancel();
    }
}

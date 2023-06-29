using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Util.Method;

public class RotatingImage : MonoBehaviour
{
    private CancellationTokenSource _cancellationTokenSource;
    private CancellationToken _cancellationToken;
    public CancellationTokenSource CancellationTokenSource { get => _cancellationTokenSource; }
    private RectTransform _rectTransform;
    private Vector3 _clockWiseDirection = Vector3.back;
    private float _turnSpeed = 360;

    public void StartRotation()
    {
        _rectTransform = GetComponent<RectTransform>();
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;
        Method.Rotate(_cancellationToken, _rectTransform, _clockWiseDirection, _turnSpeed).Forget();
    }

    private void OnDestroy()
    {
        _cancellationTokenSource?.Cancel();
    }
}

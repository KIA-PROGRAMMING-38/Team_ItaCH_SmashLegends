using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class AliceBomb : MonoBehaviour
{
    public Transform[] point;
    public ParticleSystem[] effect;
    private CancellationTokenSource _cancelToken;
    private Transform currentTransform;
    private BoxCollider _boxCollider;

    private bool _bezier;
    private bool _isAttack;
    private float _time;
    private Vector3[] _targetPoint;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        currentTransform = transform.root;
        _cancelToken = new CancellationTokenSource();
        _targetPoint = new Vector3[point.Length];
    }
    private void OnEnable()
    {
        for (int i = 0; i < point.Length; ++i)
        {
            _targetPoint[i] = point[i].position;
        }
    }
    private void FixedUpdate()
    {
        if (!_bezier)
        {
            float bezierSpeed = 1.5f;

            _time += Time.deltaTime * bezierSpeed;
            ThirdBezierCurve(_targetPoint, _time);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(StringLiteral.GROUND) && !_bezier)
        {
            int resetTime = 0;

            _time = resetTime;
            _bezier = true;
            transform.rotation = Quaternion.Euler(-90, 0, 0);
            PlayBombEffect().Forget();
            SetParent();
        }

        if (other.CompareTag(StringLiteral.PLAYER) && other.gameObject.layer != currentTransform.gameObject.layer)
        {
            if (_isAttack)
            {
                HitBombEffect(other).Forget();
            }
        }
    }

    private async UniTaskVoid PlayBombEffect()
    {
        int startEffectIndex = 0;
        _cancelToken = new CancellationTokenSource();

        _boxCollider.enabled = true;
        _isAttack = true;

        PlayEffect(startEffectIndex);

        await UniTask.Delay(4000, cancellationToken: _cancelToken.Token);

        PlayAllEffect();

        await UniTask.Delay(400);

        RootReCall();
        _boxCollider.enabled = false;
        _isAttack = false;
        _bezier = false;
    }

    private async UniTaskVoid HitBombEffect(Collider other)
    {
        _isAttack = false;
        CancelUniTask();
        PlayAllEffect();

        await UniTask.Delay(400);

        RootReCall();
        _boxCollider.enabled = false;
        _bezier = false;
    }

    private void ThirdBezierCurve(Vector3[] point, float time)
    {
        Vector3 transformPosition = Vector3.Lerp(Vector3.Lerp(point[0], point[1], time),
                                    Vector3.Lerp(point[2], point[3], time), time);

        transform.position = transformPosition;

        if (time < 0.6f)
        {
            transform.localRotation = Quaternion.Euler(-180, 0, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(-30, 0, 0);
        }
    }

    private void PlayAllEffect() => Array.ForEach(effect, elem => PlayEffect(elem));
    private void PlayEffect(int index) => PlayEffect(effect[index]);
    private void PlayEffect(ParticleSystem effect)
    {
        effect.gameObject.SetActive(true);
        effect.Play();
    }
    private void CancelUniTask()
    {
        _cancelToken.Cancel();
    }
    private void RootReCall()
    {
        gameObject.transform.SetParent(currentTransform);
        gameObject.SetActive(false);
    }
    private void SetParent() => gameObject.transform.SetParent(null);
}

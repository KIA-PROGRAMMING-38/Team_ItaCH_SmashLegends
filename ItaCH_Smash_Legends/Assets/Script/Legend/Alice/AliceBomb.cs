using Cysharp.Threading.Tasks;
using System;
using System.Drawing;
using System.Threading;
using TMPro;
using UnityEngine;

public class AliceBomb : MonoBehaviour
{
    // TODO 기능 재정의
    public Transform[] point;
    public ParticleSystem[] effect;
    private CharacterStatus _characterStatus;
    private CancellationTokenSource _cancelToken;
    private Transform currentTransform;
    private BoxCollider _boxCollider;

    private Vector3 _knockBackDirection { get => Vector3.up; }
    private float _knockBackPower { get => 0.8f; }
    private bool _bezier;
    private bool _isAttack;
    private float _time;
    private Transform[] _targetPoint;
    private void Awake()
    {
        _characterStatus = transform.root.GetComponent<CharacterStatus>();
        _boxCollider = GetComponent<BoxCollider>();
        currentTransform = transform.root;
        _cancelToken = new CancellationTokenSource();
    }
    private void Start()
    {
        _targetPoint = new Transform[point.Length];

        for (int i = 0; i < point.Length; ++i)
        {
            _targetPoint[i].position = point[i].position;
            Debug.Log(_targetPoint[i]);
        }
    }
    private void OnEnable()
    {

    }

    private void FixedUpdate()
    {
        float bezierSpeed = 1.5f;
        if (!_bezier)
        {
            _time += Time.deltaTime * bezierSpeed;
            ThirdBezierCurve(_targetPoint, _time);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        int resetTime = 0;
        if (other.CompareTag(StringLiteral.GROUND) && !_bezier)
        {
            _time = resetTime;
            _bezier = true;
            transform.rotation = Quaternion.Euler(-90, 0, 0);
            PlayBombEffect().Forget();
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

        SetParent();
        _boxCollider.enabled = true;
        PlayEffect(startEffectIndex);
        _isAttack = true;
        await UniTask.Delay(4000, cancellationToken: _cancelToken.Token);
        PlayAllEffect();
        await UniTask.Delay(400);
        RootReCall();
        _boxCollider.enabled = false;
        _isAttack = false;
        _bezier = false;
    }
    public async UniTaskVoid HitBombEffect(Collider other)
    {
        _cancelToken = new CancellationTokenSource();

        _isAttack = false;
        SetParent();
        PlayAllEffect();
        await UniTask.Delay(200);
        //_aliceHit.GetHit(_knockBackDirection, _knockBackPower, AnimationHash.Hit, other, _characterStatus.Stat.HeavyAttackDamage);
        await UniTask.Delay(400);
        CancelUniTask();
        RootReCall();
        _boxCollider.enabled = false;
        _bezier = false;
    }
    public void ThirdBezierCurve(Transform[] point, float time)
    {
        Vector3 transformPosition = Vector3.Lerp(Vector3.Lerp(point[0].position, point[1].position, time),
                                    Vector3.Lerp(point[2].position, point[3].position, time), time);

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

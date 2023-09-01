using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class AliceBomb : HitZone
{
    public Transform[] point;
    public ParticleSystem[] effect;
    private CancellationTokenSource _cancelToken;
    private Transform _currentTransform;
    private BoxCollider _boxCollider;
    private bool _isbezier;
    private bool _isAttack;
    private float _time;
    private float _groundEnterTime = 1f;
    private Vector3[] _targetPoint;
    public Vector3 ConstructorForward { get; set; }

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _currentTransform = transform.root;
        _cancelToken = new CancellationTokenSource();
        _targetPoint = new Vector3[point.Length];
        legendController = _currentTransform.GetComponent<LegendController>();
        gameObject.SetActive(false);
    }

    private void Start()
    {
        DamageAmount = legendController.Stat.HeavyAttackDamage;
        knockbackPower = legendController.Stat.HeavyKnockbackPower;
        knockbackDirection = _currentTransform.up;
        AnimationType = AnimationHash.Hit;
        gameObject.layer = LayerMask.NameToLayer(legendController.GetChildLayer());
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
        if (_isbezier == false)
        {
            float bezierSpeed = 1.5f;

            _time += Time.deltaTime * bezierSpeed;
            ThirdBezierCurve(_targetPoint, _time);

            if (_time >= _groundEnterTime)
            {
                _isbezier = true;
            }
        }

        if (_isbezier && _isAttack == false)
        {
            _time = 0;
            transform.rotation = Quaternion.Euler(-90, 0, 0);
            SetParent();
            Managers.SoundManager.Play(SoundType.SFX, StringLiteral.SFX_MINE_SET, legend: LegendType.Alice);
            PlayBombEffect().Forget();
        }
    }

    private void OnDisable()
    {
        transform.position = _currentTransform.position;
        InitBombConditions();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isAttack && other.CompareTag(StringLiteral.PLAYER))
        {
            ConstructorForward = _currentTransform.forward;

            HitBombEffect().Forget();
        }
    }

    private async UniTaskVoid PlayBombEffect()
    {
        int startEffectIndex = 0;
        _isAttack = true;
        _cancelToken = new CancellationTokenSource();

        _boxCollider.enabled = true;

        PlayEffect(startEffectIndex);

        await UniTask.Delay(1400);
        Managers.SoundManager.Play(SoundType.SFX, StringLiteral.SFX_MINE_ACTIVATE, legend: LegendType.Alice);

        await UniTask.Delay(2600, cancellationToken: _cancelToken.Token);

        PlayAllEffect();
        Managers.SoundManager.Play(SoundType.SFX, StringLiteral.SFX_MINE_EXPLODE, legend: LegendType.Alice);

        await UniTask.Delay(400);

        RootReCall();
    }

    private async UniTaskVoid HitBombEffect()
    {
        CancelUniTask();
        PlayAllEffect();
        Managers.SoundManager.Play(SoundType.SFX, StringLiteral.SFX_MINE_EXPLODE, legend: LegendType.Alice);

        await UniTask.Delay(400);

        RootReCall();
    }
    private void InitBombConditions()
    {
        _boxCollider.enabled = false;
        _isbezier = false;
        _isAttack = false;
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
        gameObject.transform.SetParent(_currentTransform);
        gameObject.SetActive(false);
    }
    private void SetParent() => gameObject.transform.SetParent(null);
}

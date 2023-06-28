using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class AliceBomb : MonoBehaviour
{
    public Transform[] point;
    public ParticleSystem[] effect;
    private CharacterStatus _characterStatus;
    private CancellationTokenSource _cancelToken;
    private Transform currentTransform;

    private Vector3 _knockBackDirection { get => Vector3.up; }
    private float _knockBackPower { get => 0.8f; }
    private bool _bezier { get; set; }
    private bool _isAttack { get; set; }
    private float _time;

    private void Awake()
    {
        _characterStatus = GetComponent<CharacterStatus>();
        currentTransform = transform.root;
        _cancelToken = new CancellationTokenSource();
    }
    private void FixedUpdate()
    {
        float bezierSpeed = 1.5f;
        if (!_bezier)
        {
            _time += Time.deltaTime * bezierSpeed;
            ThirdBezierCurve(point, _time);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") && !_bezier)
        {
            _time = 0;
            _bezier = true;
            transform.rotation = Quaternion.Euler(-90, 0, 0);
            PlayEffect().Forget();
        }
        if (other.CompareTag("Player") && other.gameObject.layer != currentTransform.gameObject.layer)
        {
            if (_isAttack)
            {
                HitEffect(other).Forget();
            }
        }
    }
    private async UniTaskVoid PlayEffect()
    {
        gameObject.transform.SetParent(null);
        effect[0].gameObject.SetActive(true);
        effect[0].Play();
        _isAttack = true;
        await UniTask.Delay(4000, cancellationToken: _cancelToken.Token);
        EffectPlay();
        await UniTask.Delay(400);
        RootReCall();
        _isAttack = false;
        _bezier = false;
    }
    public async UniTaskVoid HitEffect(Collider other)
    {
        _isAttack = false;
        gameObject.transform.SetParent(null);
        EffectPlay();
        BombHit(other, AnimationHash.Hit);
        //_aliceHit.GetHit(_knockBackDirection * 3, _knockBackPower, AnimationHash.Hit, other, _characterStatus.HeavyAttackDamage);
        await UniTask.Delay(400);
        CancelUniTask();
        RootReCall();
        _bezier = false;
    }
    private void BombHit(Collider other, int AnimationHash)
    {
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        Animator animator = other.GetComponent<Animator>();
        rigidbody.AddForce(_knockBackDirection * _knockBackPower, ForceMode.Impulse);
        animator.SetTrigger(AnimationHash);
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
    private void EffectPlay()
    {
        for(int index = 1; index < effect.Length; ++index)
        {
            effect[index].Play();
        }
    }
    private void CancelUniTask()
    {
        _cancelToken.Cancel();
        _cancelToken = new CancellationTokenSource();
    }
    private void RootReCall()
    {
        gameObject.transform.SetParent(currentTransform);
        gameObject.SetActive(false);
    }
}

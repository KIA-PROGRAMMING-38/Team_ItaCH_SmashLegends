using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ComboAttackType
{
    First,
    Second,
    FirstJump,
    SecondJump
}

public enum ActionType
{
    Move,
    Jump,
    DefaultAttack,
    HeavyAttack,
    SkillAttack
}

public enum KnockbackType
{
    Default,
    Heavy
}

public enum RollingDirection
{
    Front,
    Back
}

public class LegendController : MonoBehaviour
{
    // 추후 Character base 데이터 로 변환해야함.
    private float _jumpAcceleration = 14.2f;
    private float _gravitationalAcceleration = 36f;
    private float _maxFallingSpeed = 23f;
    public static readonly float MAX_JUMP_POWER = 1f;
    //
    public CancellationTokenSource TaskCancel;

    [SerializeField] private SphereCollider _skillAttackHitZone;
    [SerializeField] private SphereCollider _attackHitZone;
    [SerializeField] private SphereCollider _heavyAttackHitZone;
    [SerializeField] private BoxCollider _jumpAttackHitZone;

    private InputAction[] _actions;

    private CharacterStatus _characterStatus;
    private Rigidbody _rigidbody;
    private UnityEngine.InputSystem.PlayerInput _input;
    private LegendAnimationController _legendAnimationController;
    private EffectController _effectController;
    private Collider _collider;

    public Vector3 MoveDirection { get; private set; }
    public Vector3 RollingForward { get; private set; }

    private float _rollingDashPower = 1.2f;
    private bool _canAttack;

    private readonly Vector3 JUMP_DIRECTION = Vector3.up;

    private void Awake()
    {
        InitComponent();
        InitActions();

        Physics.gravity = new Vector3(0f, -_gravitationalAcceleration, 0f);
        _rigidbody.mass = MAX_JUMP_POWER / _jumpAcceleration;
    }

    private void FixedUpdate()
    {
        FixedMaxFallSpeed();
    }

    #region Input Event
    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        if (input != null)
        {
            MoveDirection = new Vector3(input.x, 0, input.y);
        }
    }
    private void OnJump()
    {
        _legendAnimationController.SetTrigger(AnimationHash.Jump);
    }
    private void OnDefaultAttack()
    {
        _legendAnimationController.SetTrigger(AnimationHash.FirstAttack);
    }
    private void OnSmashAttack()
    {
        _legendAnimationController.SetTrigger(AnimationHash.HeavyAttack);
    }
    private void OnSkillAttack()
    {
        _legendAnimationController.SetTrigger(AnimationHash.SkillAttack);
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(StringLiteral.Ground))
        {
            _legendAnimationController.SetBool(AnimationHash.JumpDown, false);
            _legendAnimationController.SetTrigger(AnimationHash.HitDown);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringLiteral.HangZone))
        {
            transform.forward = SetHangRotation(other.transform.position);
            transform.position = SetHangPosition(other);
            OnConstraints();

            _legendAnimationController.Play(AnimationHash.Hang);
        }

        if (other.CompareTag(StringLiteral.DefaultHit))
        {
            RollingForward = -1 * other.transform.forward;
            GetKnockbackOnAttack(other, AnimationHash.Hit, KnockbackType.Default);
        }

        if (other.CompareTag(StringLiteral.HeavyHit))
        {
            RollingForward = -1 * other.transform.forward;
            GetKnockbackOnAttack(other, AnimationHash.HitUp, KnockbackType.Heavy);
        }
    }

    private void InitComponent()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _input = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        _characterStatus = GetComponent<CharacterStatus>();
        _legendAnimationController = GetComponent<LegendAnimationController>();
        _effectController = GetComponent<EffectController>();
        _collider= GetComponent<Collider>();
    }
    private void InitActions()
    {
        _actions = new InputAction[StringLiteral.Actions.Length];

        for (int i = 0; i < _actions.Length; ++i)
        {
            _actions[i] = _input.actions[StringLiteral.Actions[i]];
        }
    }
    private void FixedMaxFallSpeed()
    {
        if (IsFalling())
        {
            Vector3 currentVelocity = _rigidbody.velocity;

            if (currentVelocity.magnitude > _maxFallingSpeed)
            {
                currentVelocity = currentVelocity * _maxFallingSpeed / currentVelocity.magnitude;
                _rigidbody.velocity = currentVelocity;
            }
        }
    }

    public void PlayComboAttack(ComboAttackType comboAttackType)
    {
        if (_canAttack && IsTriggered(ActionType.DefaultAttack))
        {
            _legendAnimationController.AttackAnimation(comboAttackType);
            LookForwardOnAttack();
            _canAttack = false;
        }
    }
    public void SetComboPossibleOnAnimationEvent(ComboAttackType type)
    {
        _legendAnimationController.SetNextAnimationClip(type);
        _canAttack = true;
    }
    public void SetComboImpossibleOnAnimationEvent() => _canAttack = false;
    public void MoveAndRotate()
    {
        if (MoveDirection != Vector3.zero)
        {
            _legendAnimationController.SetBool(AnimationHash.Run, true);
            transform.rotation = Quaternion.LookRotation(MoveDirection);
            //transform.Translate(Vector3.forward * (_characterStatus.Stat.MoveSpeed * Time.deltaTime));
            // 데이터 연동 전 임시 코드
            transform.Translate(Vector3.forward * (5.3f * Time.deltaTime));
        }
        else
        {
            _legendAnimationController.SetBool(AnimationHash.Run, false);
        }
    }
    private void LookForwardOnAttack()
    {
        if (MoveDirection != Vector3.zero)
        {
            transform.forward = MoveDirection;
        }
    }

    public bool IsTriggered(ActionType actionType) => _actions[(int)actionType].triggered;
    public bool IsFalling() => _rigidbody.velocity.y <= -1f;
    public bool IsFallingOnHitUp() => _rigidbody.velocity.y <= -5f;

    public void ResetVelocity()
    {
        _rigidbody.velocity = Vector3.zero;
    }
    public void OnJumping()
    {
        _rigidbody.AddForce(JUMP_DIRECTION * MAX_JUMP_POWER, ForceMode.Impulse);
    }

    #region 각 공격별 HitZone 생성
    private void EnableAttackHitZone() => _attackHitZone.enabled = true;
    private void DisableAttackHitZone() => _attackHitZone.enabled = false;
    private void EnableJumpAttackHitZone() => _jumpAttackHitZone.enabled = true;
    private void DisableJumpAttackHitZone() => _jumpAttackHitZone.enabled = false;
    private void EnableHeavyAttackHitZone() => _heavyAttackHitZone.enabled = true;
    private void DisableHeavyAttackHitZone() => _heavyAttackHitZone.enabled = false;
    private void EnableSkillAttackHitZone() => _skillAttackHitZone.enabled = true;
    private void DisableSkillAttackHitZone() => _skillAttackHitZone.enabled = false;
    #endregion

    #region Legend DownIdle
    public async UniTaskVoid StartRollingTask()
    {
        await UniTask.WaitUntil(() => MoveDirection != Vector3.zero);

        SetRollingDirection();
        _effectController.StartInvincibleFlashEffet(_effectController.FLASH_COUNT).Forget();
    }
    public void DashOnRollUp()
    {
        _rigidbody.AddForce(MoveDirection * _rollingDashPower, ForceMode.Impulse);
    }
    private void SetRollingDirection()
    {
        if (MoveDirection != Vector3.zero)
        {
            if (MoveDirection.x != 0 && MoveDirection.z != 0)
            {
                SetDiagonalRollingDirection();
                return;
            }
            else if (RollingForward == MoveDirection)
            {
                SetRollingType(RollingDirection.Back);
                return;
            }
            else
            {
                SetRollingType(RollingDirection.Front);
                return;
            }
        }
    }
    private void SetDiagonalRollingDirection()
    {
        var rollingDirection = GetDirection(RollingForward, MoveDirection);

        SetRollingType(rollingDirection);
    }
    private void SetRollingType(RollingDirection rollingDirection)
    {
        switch (rollingDirection)
        {
            case RollingDirection.Front:
                _legendAnimationController.SetTrigger(AnimationHash.RollUpFront);
                transform.forward = MoveDirection;
                break;
            case RollingDirection.Back:
                _legendAnimationController.SetTrigger(AnimationHash.RollUpBack);
                transform.forward = -1 * MoveDirection;
                break;
            default:
                Debug.LogError("RollingDirection 미구현");
                break;
        }
    }
    private RollingDirection GetDirection(Vector2 rollingForward, Vector2 moveDirection)
    {
        if (rollingForward.x > 0)
        {
            if (moveDirection.x > 0)
            {
                return RollingDirection.Back;
            }
            else
            {
                return RollingDirection.Front;
            }
        }
        else
        {
            if (moveDirection.x < 0)
            {
                return RollingDirection.Back;
            }
            else
            {
                return RollingDirection.Front;
            }
        }
    }
    #endregion

    #region Legend Hit 
    public void GetKnockbackOnAttack(Collider other, int animationHash, KnockbackType type)
    {
        transform.forward = -1 * other.transform.forward;
        Vector3 _knockbackDirection = other.transform.forward + transform.up;

        _legendAnimationController.SetTrigger(animationHash);
        _rigidbody.AddForce(_knockbackDirection * SetKnockbackPower(type, other), ForceMode.Impulse);
    }
    private float SetKnockbackPower(KnockbackType type, Collider other)
    {
        // 스탯 연동시 적용
        //CharacterStatus otherStatus = other.GetComponent<CharacterStatus>();
        float knockbackPower = 0;

        switch (type)
        {
            case KnockbackType.Default:
                //knockbackPower = otherStatus.Stat.DefaultKnockbackPower;
                knockbackPower = 0.3f;
                break;

            case KnockbackType.Heavy:
                //knockbackPower = otherStatus.Stat.HeavyKnockbackPower;
                knockbackPower = 0.5f;
                break;
        }

        return knockbackPower;
    }
    #endregion
    
    #region Legend Hang
    private Vector3 SetHangRotation(Vector3 other)
    {
        Vector3 otherPosition = other.normalized;
        otherPosition.x = Mathf.Round(other.x);
        otherPosition.y = 0;
        otherPosition.z = Mathf.Round(other.z);

        return otherPosition * -1;
    }
    private Vector3 SetHangPosition(Collider other)
    {

        float _hangPositionY = 2.5f;
        float[] hangPosition = new float[2];
        hangPosition[0] = other.transform.position.x;
        hangPosition[1] = other.transform.position.z;

        for (int i = 0; i < hangPosition.Length; ++i)
        {
            if (hangPosition[i] > 0)
            {
                hangPosition[i] -= 0.5f;
            }
            if (hangPosition[i] < 0)
            {
                hangPosition[i] += 0.5f;
            }
        }
        Vector3 setPosition = Vector3.zero;

        if (hangPosition[0] != 0)
        {
            setPosition = new Vector3(hangPosition[0], _hangPositionY, transform.position.z);
        }
        if (hangPosition[1] != 0)
        {
            setPosition = new Vector3(transform.position.x, _hangPositionY, hangPosition[1]);
        }

        return setPosition;
    }
    private void OnConstraints()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }
    public void OffConstraints()
    {
        _rigidbody.constraints = RigidbodyConstraints.None;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }
    public async UniTaskVoid OnFalling(Animator animator)
    {
        float _fallingWaitTime = 3f;
        TaskCancel = new();
        await UniTask.Delay(TimeSpan.FromSeconds(_fallingWaitTime), cancellationToken: TaskCancel.Token);
        _collider.enabled = false;
        animator.Play(AnimationHash.HangFalling);
    }
    #endregion
}
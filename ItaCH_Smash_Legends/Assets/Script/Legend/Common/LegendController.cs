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
    // TODO : 캐릭터 스탯 적용시 변경
    private float _jumpAcceleration = 14.2f;
    private float _gravitationalAcceleration = 36f;
    private float _maxFallingSpeed = 23f;
    public static readonly float MAX_JUMP_POWER = 1f;
    //
    private CancellationTokenSource _taskCancel;

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

    private Vector3 _moveDirection;
    private Vector3 _facingDirection;
    private Vector3 _vectorZero = Vector3.zero;
    private const float ROLLING_DASH_POWER = 1.2f;
    private bool _canAttack;


    private void Awake()
    {
        InitComponent();
        InitActions();

        _rigidbody.mass = MAX_JUMP_POWER / _jumpAcceleration;
    }

    private void FixedUpdate()
    {
        LimitMaxFallingSpeedInJump();
    }

    #region Input Event
    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        if (input != null)
        {
            _moveDirection = new Vector3(input.x, 0, input.y);
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
        if (collision.gameObject.CompareTag(StringLiteral.GROUND))
        {
            _legendAnimationController.SetBool(AnimationHash.JumpDown, false);
            _legendAnimationController.SetTrigger(AnimationHash.HitDown);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringLiteral.HANGZONE))
        {
            ResetVelocity();
            transform.forward = GetHangForward(other.transform.position);
            transform.position = GetHangPosition(other.transform.position);
            _legendAnimationController.Play(AnimationHash.Hang);
        }

        if (other.CompareTag(StringLiteral.DEFAULT_HIT))
        {
            _facingDirection = -1 * other.transform.forward;
            SetKnockbackOnAttack(other, AnimationHash.Hit, KnockbackType.Default);
        }

        if (other.CompareTag(StringLiteral.HEAVY_HIT))
        {
            _facingDirection = -1 * other.transform.forward;
            SetKnockbackOnAttack(other, AnimationHash.HitUp, KnockbackType.Heavy);
        }
    }

    private void InitComponent()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _input = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        _characterStatus = GetComponent<CharacterStatus>();
        _legendAnimationController = GetComponent<LegendAnimationController>();
        _effectController = GetComponent<EffectController>();
        _collider = GetComponent<Collider>();
    }
    private void InitActions()
    {
        _actions = new InputAction[StringLiteral.ACTIONS.Length];

        for (int i = 0; i < _actions.Length; ++i)
        {
            _actions[i] = _input.actions[StringLiteral.ACTIONS[i]];
        }
    }

    private void LimitMaxFallingSpeedInJump()
    {
        void SetMaxSpeed()
        {

            Vector3 currentVelocity = _rigidbody.velocity;

            currentVelocity = currentVelocity * _maxFallingSpeed / currentVelocity.magnitude;
            _rigidbody.velocity = currentVelocity;
        }
        bool IsExceededSpeed() => _rigidbody.velocity.magnitude > _maxFallingSpeed;

        if (IsFalling())
        {
            if (IsExceededSpeed())
            {
                SetMaxSpeed();
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

    public bool IsTriggered(ActionType actionType) => _actions[(int)actionType].triggered;
    public bool IsFalling() => _rigidbody.velocity.y <= -1f;
    public bool IsFallingOnHitUp() => _rigidbody.velocity.y <= -5f;

    public void MoveAndRotate()
    {
        if (_moveDirection != _vectorZero)
        {
            _legendAnimationController.SetBool(AnimationHash.Run, true);
            transform.rotation = Quaternion.LookRotation(_moveDirection);
            // TODO : 캐릭터 스탯 적용후 변경
            //transform.Translate(Vector3.forward * (_characterStatus.Stat.MoveSpeed * Time.deltaTime));
            transform.Translate(Vector3.forward * (5.3f * Time.deltaTime));
        }
        else
        {
            _legendAnimationController.SetBool(AnimationHash.Run, false);
        }
    }
    private void LookForwardOnAttack()
    {
        if (_moveDirection != _vectorZero)
        {
            transform.forward = _moveDirection;
        }
    }
    public void ResetVelocity()
    {
        _rigidbody.velocity = _vectorZero;
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

    public void SetRollingDirection()
    {
        if (_moveDirection == _vectorZero)
        {
            return;
        }
        else if (_moveDirection.x != 0 && _moveDirection.z != 0)
        {
            SetDiagonalRollingDirection();
            return;
        }
        else if (_facingDirection == _moveDirection)
        {
            SetRollingType(RollingDirection.Front);
            return;
        }
        else
        {
            SetRollingType(RollingDirection.Back);
            return;
        }
        void SetRollingType(RollingDirection rollingDirection)
        {
            switch (rollingDirection)
            {
                case RollingDirection.Front:
                    _legendAnimationController.SetTrigger(AnimationHash.RollUpFront);
                    transform.forward = _moveDirection;
                    break;
                case RollingDirection.Back:
                    _legendAnimationController.SetTrigger(AnimationHash.RollUpBack);
                    transform.forward = -1 * _moveDirection;
                    break;
                default:
                    Debug.LogError("RollingDirection 미구현");
                    break;
            }
        }
        void SetDiagonalRollingDirection()
        {
            RollingDirection rollingDirection = GetDirection(_facingDirection, _moveDirection);
            SetRollingType(rollingDirection);

            RollingDirection GetDirection(Vector2 rollingForward, Vector2 moveDirection)
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
        }
    }
    public void DashOnRollUp()
    {
        _rigidbody.AddForce(_moveDirection * ROLLING_DASH_POWER, ForceMode.Impulse);
    }

    public void SetKnockbackOnAttack(Collider other, int animationHash, KnockbackType type)
    {
        transform.forward = _facingDirection;

        Vector3 knockbackDirection = other.transform.forward + transform.up;

        _legendAnimationController.SetTrigger(animationHash);
        _rigidbody.AddForce(knockbackDirection * GetKnockbackPower(type, other), ForceMode.Impulse);

        float GetKnockbackPower(KnockbackType type, Collider other)
        {
            // TODO : 스탯 연동 시 적용
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
    }

    private Vector3 GetHangForward(Vector3 other)
    {
        Vector3 otherPosition = other.normalized;
        otherPosition.x = Mathf.Round(other.x);
        otherPosition.y = 0;
        otherPosition.z = Mathf.Round(other.z);

        return otherPosition * -1;
    }
    private Vector3 GetHangPosition(Vector3 hangPosition)
    {
        float hangPositionY = 2.5f;

        if (hangPosition.x != 0)
        {
            hangPosition = new Vector3(GetHangCorrectionPosition(hangPosition.x), hangPositionY, transform.position.z);
        }

        else if (hangPosition.z != 0)
        {
            hangPosition = new Vector3(transform.position.x, hangPositionY, GetHangCorrectionPosition(hangPosition.z));
        }

        return hangPosition;

        float GetHangCorrectionPosition(float value)
        {
            if (value > 0)
            {
                value -= 0.5f;
            }

            if (value < 0)
            {
                value += 0.5f;
            }

            return value;
        }
    }
    public async UniTaskVoid FallAsync(Animator animator)
    {
        float _fallingWaitTime = 3f;
        _taskCancel = new();

        await UniTask.Delay(TimeSpan.FromSeconds(_fallingWaitTime), cancellationToken: _taskCancel.Token);
        _collider.enabled = false;
        animator.Play(AnimationHash.HangFalling);
    }
    public void EscapeInHang()
    {
        _taskCancel.Cancel();
    }
}
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

public enum SinglePlayController
{
    Controller_1P,
    Controller_2P
}

public class LegendController : MonoBehaviour
{
    public const float MAX_JUMP_POWER = 1f;
    private CancellationTokenSource _taskCancel;

    private InputAction[] _actions;

    public int OwnerUserID { get; private set; }

    public LegendStatData Stat { get; set; }
    public float HPRatio { get; set; } // TO DO : 체력 관리

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

    public void Init(UserData user)
    {
        GetComponents();
        SetLegendStat(user.SelectedLegend);
        SetController(user.ID);
        InitActions();
        SetRigidbody();
        user.OwnedLegend = this;
        OwnerUserID = user.ID;
    }

    private void GetComponents()
    {
        _rigidbody = Utils.GetOrAddComponent<Rigidbody>(this.gameObject);
        _input = Utils.GetOrAddComponent<UnityEngine.InputSystem.PlayerInput>(this.gameObject);
        _legendAnimationController = Utils.GetOrAddComponent<LegendAnimationController>(this.gameObject);
        _effectController = GetComponent<EffectController>();
        _collider = GetComponent<Collider>();
    }

    private void SetLegendStat(LegendType legendIndex) => Stat = Managers.DataManager.LegendStats[(int)legendIndex].Clone();

    private void SetController(int userID) // TO DO : 피격 로직 수정 이후 죽었을 때 이벤트에서 다시 호출 필요
    {
        switch ((SinglePlayController)userID)
        {
            case SinglePlayController.Controller_1P:
                _input.SwitchCurrentActionMap(StringLiteral.FIRST_PLAYER_ACTIONS);
                break;

            case SinglePlayController.Controller_2P:
                _input.actions.name = StringLiteral.PLAYER_INPUT;
                _input.SwitchCurrentActionMap(StringLiteral.SECOND_PLAYER_ACTIONS);
                Keyboard keyBoard = InputSystem.GetDevice<Keyboard>();
                _input.actions.devices = new InputDevice[] { keyBoard };
                break;

            default:
                return;
        }
    }

    private void InitActions()
    {
        _actions = new InputAction[StringLiteral.ACTIONS.Length];

        for (int i = 0; i < _actions.Length; ++i)
        {
            _actions[i] = _input.actions[StringLiteral.ACTIONS[i]];
        }
    }

    private void SetRigidbody()
    {
        _rigidbody.mass = MAX_JUMP_POWER / Stat.JumpAcceleration;
        _rigidbody.drag = 0.5f;
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
            if (other.name == StringLiteral.ALICE_BOMB)
            {
                AliceBomb bomb = other.GetComponent<AliceBomb>();
                _facingDirection = -1 * bomb.ConstructorForward;
            }
            else
            {
                _facingDirection = -1 * other.transform.forward.normalized;
            }

            SetKnockbackOnAttack(other, AnimationHash.HitUp, KnockbackType.Heavy);
        }
    }

    private void LimitMaxFallingSpeedInJump()
    {
        void SetMaxSpeed()
        {

            Vector3 currentVelocity = _rigidbody.velocity;

            currentVelocity = currentVelocity * Stat.MaxFallingSpeed / currentVelocity.magnitude;
            _rigidbody.velocity = currentVelocity;
        }
        bool IsExceededSpeed() => _rigidbody.velocity.magnitude > Stat.MaxFallingSpeed;

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
            transform.Translate(Vector3.forward * (Stat.MoveSpeed * Time.deltaTime));
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
    public Vector3 GetMoveDirection()
    {
        return _moveDirection;
    }
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
        HitZone otherHit = other.GetComponent<HitZone>();

        _legendAnimationController.SetTrigger(otherHit.GetAnimationKind());
        otherHit.SetKnockback(_rigidbody);
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

    public string GetChildLayer()
    {
        if (this.gameObject.layer == LayerMask.NameToLayer("TeamRed"))
        {
            return "TeamRedHitZone";
        }
        else
        {
            return "TeamBlueHitZone";
        }
    }
    // TO DO : Damage 로직 추가 필요
    public void Damage(int damage) // 구버전
    {
        //    int damagedHealthPoint = _currentHealthPoint - damage;
        //    _currentHealthPoint = Mathf.Max(damagedHealthPoint, DEAD_TRIGGER_HP);
        //    OnPlayerHealthPointChange.Invoke(_currentHealthPoint, CurrentHPRatio);
        //    OnPlayerGetDamage?.Invoke(damage);
        //    if (_currentHealthPoint <= DEAD_TRIGGER_HP && !this._isDead)        
    }

    // TO DO : Die() 구현
}

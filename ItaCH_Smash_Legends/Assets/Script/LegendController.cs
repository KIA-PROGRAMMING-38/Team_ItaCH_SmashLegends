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
public class LegendController : MonoBehaviour
{
    private float _jumpAcceleration = 14.2f;
    private float _gravitationalAcceleration = 36f;
    private float _maxFallingSpeed = 23f;
    public static readonly float MAX_JUMP_POWER = 1f;

    [SerializeField] private SphereCollider _skillAttackHitZone;
    [SerializeField] private SphereCollider _attackHitZone;
    [SerializeField] private SphereCollider _heavyAttackHitZone;
    [SerializeField] private BoxCollider _jumpAttackHitZone;

    private InputAction[] _actions;

    private CharacterStatus _characterStatus;
    private Rigidbody _rigidbody;
    private UnityEngine.InputSystem.PlayerInput _input;
    private PlayerHit _playerHit;
    private LegendAnimationController _legendAnimationController;
    public Vector3 MoveDirection { get; private set; }

    internal bool isJump = true;
    private bool _canAttack;

    // 추후 Character base 데이터 로 변환해야함.
    //private const float MAX_JUMP_POWER = 1f;
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
        _legendAnimationController.Animator.SetTrigger(AnimationHash.Jump);
    }
    private void OnDefaultAttack()
    {
        _legendAnimationController.Animator.SetTrigger(AnimationHash.FirstAttack);
    }
    private void OnSmashAttack()
    {
        _legendAnimationController.Animator.SetTrigger(AnimationHash.HeavyAttack);
    }
    private void OnSkillAttack()
    {
        _legendAnimationController.Animator.SetTrigger(AnimationHash.SkillAttack);
    }
    private void InitComponent()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _input = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        _characterStatus = GetComponent<CharacterStatus>();
        _legendAnimationController = GetComponent<LegendAnimationController>();
        _playerHit = GetComponent<PlayerHit>();
    }
    private void InitActions()
    {
        _actions = new InputAction[StringLiteral.Actions.Length];

        for (int i = 0; i < _actions.Length; ++i)
        {
            _actions[i] = _input.actions[StringLiteral.Actions[i]];
        }
    }

    public bool IsTriggered(ActionType actionType) => _actions[(int)actionType].triggered;
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
    private void LookForwardOnAttack()
    {
        if (MoveDirection != Vector3.zero)
        {
            transform.forward = MoveDirection;
        }
    }
    public void MoveAndRotate()
    {
        if (MoveDirection != Vector3.zero)
        {
            _legendAnimationController.Animator.SetBool(AnimationHash.Run, true);
            transform.rotation = Quaternion.LookRotation(MoveDirection);
            transform.Translate(Vector3.forward * (5.3f * Time.deltaTime));
        }
        else
        {
            _legendAnimationController.Animator.SetBool(AnimationHash.Run, false);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(StringLiteral.Ground))
        {
            _legendAnimationController.Animator.SetBool(AnimationHash.JumpDown, false);
            isJump = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringLiteral.HangZone))
        {
            // TODO : 추후구현
            // Animation.Play(Hang);
        }

        if (other.CompareTag(StringLiteral.DefaultHit))
        {
            _playerHit.GetKnockbackOnAttack(other, AnimationHash.Hit, KnockbackType.Default);
        }
        if (other.CompareTag(StringLiteral.HeavyHit))
        {
            _playerHit.GetKnockbackOnAttack(other, AnimationHash.HitUp, KnockbackType.Heavy);
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
    public bool IsFalling() => _rigidbody.velocity.y <= -1f;
    
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
    // PlayerRollUp 
    // DownIdle 상태에서 기능 수행

    // PlayerHangController
    // Hang 상태에서 입력시 처리
}
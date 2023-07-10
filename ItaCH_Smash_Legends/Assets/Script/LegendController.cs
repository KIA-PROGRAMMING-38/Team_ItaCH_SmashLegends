using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ComboAttackType
{
    First,
    Second,
    FirstJump,
    SecondJump,
}
public enum ActionType
{
    Move,
    Jump,
    DefaultAttack,
    HeavyAttack,
    SkillAttack
}

public class LegendController : MonoBehaviour
{
    //추후 리소스 할당
    [SerializeField]
    private AnimationClip[] _applyAttackClip;
    [SerializeField]
    private AnimationClip[] _applyJumpAttackClip;


    private AnimationClip[] _applyAnimationClip;
    private InputAction[] _actions;
    private Animator _animator;
    private AnimatorOverrideController _animatorOverrideController;
    private CharacterStatus _characterStatus;
    private Rigidbody _rigidbody;
    private UnityEngine.InputSystem.PlayerInput _input;

    private ActionType[] _actionType = (ActionType[])Enum.GetValues(typeof(ActionType));

    // 추후 리소스로 매니저로 캐싱 후 사용
    // Character Type 으로 변환해야함.
    private string[] _overrideAnimatorName;

    public Vector3 MoveDirection { get; private set; }
    private int _animationClipIndex;

    private bool _canAttack;
    private const float MAX_JUMP_POWER = 1f;
    private readonly Vector3 JUMP_DIRECTION = Vector3.up;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _input = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        _characterStatus = GetComponent<CharacterStatus>();

        _actions = new InputAction[StringLiteral.ActionLiteral.Length];

        SetAnimatorClip();
    }
    private void Start()
    {
        InitActions();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringLiteral.HangZone))
        {
            // Animation.Play(Hang);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(StringLiteral.Ground))
        {
            _animator.SetBool(AnimationHash.JumpDown, false);
        }
    }
    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        if (input != null)
        {
            MoveDirection = new Vector3(input.x, 0, input.y);
            _animator.SetBool(AnimationHash.Run, true);
        }
    }
    private void OnJump() { }
    private void OnDefaultAttack() { }
    private void OnSmashAttack() { }
    private void OnSkillAttack() { }

    private void InitActions()
    {
        for (int i = 0; i < _actions.Length; ++i)
        {
            _actions[i] = _input.actions[StringLiteral.ActionLiteral[i]];
        }
    }
    public void SetNextAnimation()
    {
        foreach (ActionType actionType in _actionType)
        {
            if (IsTriggered(actionType))
            {
                PlayNextAnimation(actionType);

                return;
            }
        }
    }
    public void PlayJumpAttackAnimation()
    {
        if (IsTriggered(ActionType.DefaultAttack))
        {
            _animator.Play(AnimationHash.FirstJumpAttack);
        }
    }
    private bool IsTriggered(ActionType actionType) => _actions[(int)actionType].triggered;
    private void PlayNextAnimation(ActionType actionType)
    {
        switch (actionType)
        {
            case ActionType.Move:
                _animator.Play(AnimationHash.Run);
                break;
            case ActionType.Jump:
                _animator.Play(AnimationHash.Jump);
                break;
            case ActionType.DefaultAttack:
                _animator.Play(AnimationHash.FirstAttack);
                break;
            case ActionType.HeavyAttack:
                _animator.Play(AnimationHash.HeavyAttack);
                break;
            case ActionType.SkillAttack:
                _animator.Play(AnimationHash.SkillAttack);
                break;
        }
    }
    public void PlayComboAttack(ComboAttackType comboAttackType)
    {
        if (_canAttack && IsTriggered(ActionType.DefaultAttack))
        {
            PlayAttackAnimation(comboAttackType);
            LookForwardOnAttack();
            _canAttack = false;
        }
    }
    private void PlayAttackAnimation(ComboAttackType comboAttackType)
    {
        switch (comboAttackType)
        {
            case ComboAttackType.First:
                _animator.Play(AnimationHash.FirstAttack);
                break;
            case ComboAttackType.Second:
                _animator.Play(AnimationHash.SecondAttack);
                break;
            case ComboAttackType.FirstJump:
                _animator.Play(AnimationHash.FirstJumpAttack);
                break;
            case ComboAttackType.SecondJump:
                _animator.Play(AnimationHash.SecondJumpAttack);
                break;
        }
    }
    private void SetAnimatorClip()
    {
        _overrideAnimatorName = new string[_animator.runtimeAnimatorController.animationClips.Length];
        _applyAnimationClip = new AnimationClip[_animator.runtimeAnimatorController.animationClips.Length];
        _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);

        for (int i = 0; i < _animator.runtimeAnimatorController.animationClips.Length; ++i)
        {
            _overrideAnimatorName[i] = _animatorOverrideController.animationClips[i].name;
            _applyAnimationClip[i] = _animator.runtimeAnimatorController.animationClips[i];
            _animatorOverrideController[_overrideAnimatorName[i]] = _applyAnimationClip[i];
        }

        _animator.runtimeAnimatorController = _animatorOverrideController;
    }
    private void PlayNextAnimationClip(ComboAttackType type)
    {
        if (_animationClipIndex < _applyJumpAttackClip.Length - 1)
        {
            ++_animationClipIndex;
        }
        if (type == ComboAttackType.FirstJump)
        {
            _animatorOverrideController[StringLiteral.JumpAnimationClipLiteral] = _applyJumpAttackClip[_animationClipIndex];
        }
        else
        {
            int value = _animationClipIndex % 2;
            _animatorOverrideController[StringLiteral.AnimationClipLiteral[value]] = _applyAttackClip[_animationClipIndex];
        }
    }
    public void ResetComboAttackAnimationClip()
    {
        _animationClipIndex = 0;
        _animatorOverrideController[StringLiteral.AnimationClipLiteral[0]] = _applyAttackClip[0];
        _animatorOverrideController[StringLiteral.JumpAnimationClipLiteral] = _applyJumpAttackClip[0];
    }
    public void SetComboPossibleOnAnimationEvent(ComboAttackType type)
    {
        PlayNextAnimationClip(type);
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
            transform.rotation = Quaternion.LookRotation(MoveDirection);
            transform.Translate(Vector3.forward * (_characterStatus.MoveSpeed * Time.deltaTime));
        }
    }
    public void JumpInput()
    {
        _rigidbody.AddForce(JUMP_DIRECTION * MAX_JUMP_POWER, ForceMode.Impulse);
    }
    // PlayerRollUp 
    // DownIdle 상태에서 기능 수행

    // PlayerHangController
    // Hang 상태에서 입력시 처리
}
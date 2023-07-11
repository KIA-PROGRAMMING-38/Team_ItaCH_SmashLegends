using System;
using TMPro;
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
    private InputAction[] _actions;

    private CharacterStatus _characterStatus;
    private Rigidbody _rigidbody;
    private UnityEngine.InputSystem.PlayerInput _input;

    private LegendAnimationController _legendAnimationController;

    public Vector3 MoveDirection { get; private set; }

    private bool _canAttack;

    // 추후 Character base 데이터 로 변환해야함.
    private const float MAX_JUMP_POWER = 1f;
    private readonly Vector3 JUMP_DIRECTION = Vector3.up;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _input = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        _characterStatus = GetComponent<CharacterStatus>();
        _legendAnimationController = GetComponent<LegendAnimationController>();

        InitActions();
    }

    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        if (input != null)
        {
            MoveDirection = new Vector3(input.x, 0, input.y);
        }
        else
        {
            _legendAnimationController.SetBoolAnimationFalse(AnimationHash.Run);
        }
    }

    private void OnJump()
    {
        _legendAnimationController.SetTriggerAnimation(AnimationHash.Jump);
    }
    private void OnDefaultAttack()
    {
        _legendAnimationController.SetTriggerAnimation(AnimationHash.FirstAttack);
    }
    private void OnSmashAttack()
    {
        _legendAnimationController.SetTriggerAnimation(AnimationHash.HeavyAttack);
    }
    private void OnSkillAttack()
    {
        _legendAnimationController.SetTriggerAnimation(AnimationHash.SkillAttack);
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
            _legendAnimationController.PlayAttackAnimation(comboAttackType);
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
            _legendAnimationController.SetBoolAnimationTrue(AnimationHash.Run);
            transform.rotation = Quaternion.LookRotation(MoveDirection);
            transform.Translate(Vector3.forward * (_characterStatus.Stat.MoveSpeed * Time.deltaTime));
        }
        else
        {
            _legendAnimationController.SetPlayAnimation(AnimationHash.Idle);
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
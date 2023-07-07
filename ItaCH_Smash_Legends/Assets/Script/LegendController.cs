﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ComboAttackType
{
    First,
    Second
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
    public AnimationClip[] ApplyClip;

    private AnimationClip[] _applyAnimationClip;
    private InputAction[] _action;
    private Animator _animator;
    private AnimatorOverrideController _animatorOverrideController;
    private CharacterStatus _characterStatus;
    private UnityEngine.InputSystem.PlayerInput _input;

    private ActionType[] _actionType = (ActionType[])Enum.GetValues(typeof(ActionType));
    private string[] _actionLiteral = new[] { "Move", "Jump", "DefaultAttack", "SmashAttack", "SkillAttack" };
    // 추후 스트링 리터럴 캐싱 후 사용
    private string[] _animationClipLiteral = new[] { "Peter_FirstAttack", "Peter_SecondAttack" };
    // 추후 리소스로 매니저로 캐싱 후 사용
    // Character Type 으로 변환해야함.
    private string[] _overrideAnimatorName;

    public Vector3 MoveDirection { get; private set; }
    private int _animationClipIndex;

    private bool _canAttack;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _input = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        _characterStatus = GetComponent<CharacterStatus>();

        _action = new InputAction[_actionLiteral.Length];

        _animatorOverrideController = Resources.Load<AnimatorOverrideController>(ResourcesManager.HookAnimator);

        _overrideAnimatorName = new string[_animator.runtimeAnimatorController.animationClips.Length];
        _applyAnimationClip = new AnimationClip[_animator.runtimeAnimatorController.animationClips.Length];
        _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);

        // 애니메이터 오버라이딩 후 코드
        //for (int i = 0; i < _anim.runtimeAnimatorController.animationClips.Length; ++i)
        //{

        //    SetAnimatorClip(i);
        //}

        _animator.runtimeAnimatorController = _animatorOverrideController;
    }
    private void Start()
    {
        for (int i = 0; i < _action.Length; ++i)
        {
            _action[i] = _input.actions[_actionLiteral[i]];
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HangZone"))
        {
            // Animation.Play(Hang);
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
    public void SetNextAnimation()
    {
        foreach (ActionType actionType in _actionType)
        {
            if (IsTriggered(actionType))
            {
                ChangeNextAnimation(actionType);

                return;
            }
        }
    }
    private bool IsTriggered(ActionType actionType) => _action[(int)actionType].triggered;
    private void ChangeNextAnimation(ActionType actionType)
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
        if (_canAttack && _action[(int)ActionType.DefaultAttack].triggered)
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
        }
    }
    private void SetAnimatorClip(int i)
    {
        _overrideAnimatorName[i] = _animatorOverrideController.animationClips[i].name;
        //hook.animationClips => 추후 캐릭터 Type 으로
        _applyAnimationClip[i] = _animatorOverrideController.animationClips[i];
        _animatorOverrideController[_overrideAnimatorName[i]] = _applyAnimationClip[i];
    }
    private void PlayNextAnimationClip()
    {
        if (_animationClipIndex < ApplyClip.Length - 1)
        {
            ++_animationClipIndex;
        }

        int value = _animationClipIndex % 2;
        _animatorOverrideController[_animationClipLiteral[value]] = ApplyClip[_animationClipIndex];
    }
    public void SetComboPossibleOnAnimationEvent()
    {
        PlayNextAnimationClip();
        _canAttack = true;
    }
    public void ResetComboAttack()
    {
        _animationClipIndex = 0;
        _animatorOverrideController[_animationClipLiteral[0]] = ApplyClip[0];
    }
    public void SetComboImpossibleOnAnimationEvent() => _canAttack = false;
    private void LookForwardOnAttack()
    {
        if (MoveDirection != Vector3.zero)
        {
            transform.forward = MoveDirection;
        }
    }
    public void JumpMoveAndRotate()
    {
        if (MoveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(MoveDirection);
            transform.Translate(Vector3.forward * (_characterStatus.MoveSpeed * Time.deltaTime));
        }
    }
    // PlayerRollUp 
    // DownIdle 상태에서 기능 수행

    // PlayerHangController
    // Hang 상태에서 입력시 처리
}

using System.Runtime.CompilerServices;
using UnityEngine;

public class LegendAnimationController : MonoBehaviour
{
    //추후 리소스 할당
    [SerializeField]
    private AnimationClip[] _applyAttackClip;
    [SerializeField]
    private AnimationClip[] _applyJumpAttackClip;

    private LegendController _legendController;
    private Animator _animator;
    private AnimatorOverrideController _animatorOverrideController;

    private int _animationClipIndex;
    internal bool _isHitUp;

    private void Awake()
    {
        _legendController = GetComponent<LegendController>();
        _animator = GetComponent<Animator>();

        SetAnimatorClip();
    }
    private void SetAnimatorClip()
    {
        string[] _overrideAnimatorName = new string[_animator.runtimeAnimatorController.animationClips.Length];
        AnimationClip[] _applyAnimationClip = new AnimationClip[_animator.runtimeAnimatorController.animationClips.Length];

        _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);

        for (int i = 0; i < _animator.runtimeAnimatorController.animationClips.Length; ++i)
        {
            _overrideAnimatorName[i] = _animatorOverrideController.animationClips[i].name;
            _applyAnimationClip[i] = _animator.runtimeAnimatorController.animationClips[i];
            _animatorOverrideController[_overrideAnimatorName[i]] = _applyAnimationClip[i];
        }

        _animator.runtimeAnimatorController = _animatorOverrideController;
    }
    public void SetNextAnimationClip(ComboAttackType type)
    {
        if (type == ComboAttackType.FirstJump)
        {
            if (_animationClipIndex < _applyJumpAttackClip.Length - 1)
            {
                ++_animationClipIndex;
            }

            _animatorOverrideController[StringLiteral.JumpAnimationClip] = _applyJumpAttackClip[_animationClipIndex];
        }
        else
        {
            if (_animationClipIndex < _applyAttackClip.Length - 1)
            {
                ++_animationClipIndex;
            }

            int value = _animationClipIndex % 2;
            _animatorOverrideController[StringLiteral.AnimationClip[value]] = _applyAttackClip[_animationClipIndex];
        }
    }
    public void AttackAnimation(ComboAttackType comboAttackType)
    {
        switch (comboAttackType)
        {
            case ComboAttackType.First:
                PlayAnimation(AnimationHash.FirstAttack);
                break;
            case ComboAttackType.Second:
                PlayAnimation(AnimationHash.SecondAttack);
                break;
        }
    }
    public void TriggerAnimation(int animationHash)
    {
        _animator.SetTrigger(animationHash);
    }
    public void TrueAnimation(int animationHash)
    {
        _animator.SetBool(animationHash, true);
    }
    public void FalseAnimation(int animationHash)
    {
        _animator.SetBool(animationHash, false);
    }
    public void PlayAnimation(int animationhash)
    {
        _animator.Play(animationhash);
    }
    public void ResetComboAttackAnimationClip()
    {
        _animationClipIndex = 0;
        _animatorOverrideController[StringLiteral.AnimationClip[0]] = _applyAttackClip[0];
        _animatorOverrideController[StringLiteral.JumpAnimationClip] = _applyJumpAttackClip[0];
    }
    public void ResetAllAnimatorTriggers(Animator animator)
    {
        foreach (AnimatorControllerParameter trigger in animator.parameters)
        {
            if (trigger.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(trigger.name);
            }
        }
    }
    public void LengendHit(Collider other)
    {
        Vector3 _knockbackDirection = transform.forward + transform.up;
        float power = 0.3f;
        float heavyPower = 0.5f;
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        LegendAnimationController animator = other.GetComponent<LegendAnimationController>();

        animator.TriggerAnimation(AnimationHash.Hit);
        rigidbody.AddForce(_knockbackDirection * power, ForceMode.Impulse);

        if (_isHitUp)
        {
            animator.TriggerAnimation(AnimationHash.HitUp);
            rigidbody.AddForce(_knockbackDirection * heavyPower, ForceMode.Impulse);
        }
    }
     
    private void ConvertHitUpAnimationEvent() => _isHitUp = true;
}

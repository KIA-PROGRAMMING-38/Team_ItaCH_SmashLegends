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

    private void Awake()
    {
        _legendController = GetComponent<LegendController>();
        _animator = GetComponent<Animator>();

        SetAnimatorClip();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(StringLiteral.Ground))
        {
            _animator.SetBool(AnimationHash.JumpDown, false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringLiteral.HangZone))
        {
            // Animation.Play(Hang);
        }
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
    public void PlayJumpAttackAnimation()
    {
        if (_legendController.IsTriggered(ActionType.DefaultAttack))
        {
            _animator.Play(AnimationHash.FirstJumpAttack);
        }
    }
    public void PlayAttackAnimation(ComboAttackType comboAttackType)
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
    public void SetTriggerAnimation(int animationHash)
    {
        _animator.SetTrigger(animationHash);
    }
    public void SetBoolAnimationTrue(int animationHash)
    {
        _animator.SetBool(animationHash, true);
    }
    public void SetBoolAnimationFalse(int animationHash)
    {
        _animator.SetBool(animationHash, false);
    }
    public void SetPlayAnimation(int animationHash)
    {
        _animator.Play(animationHash);
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
}

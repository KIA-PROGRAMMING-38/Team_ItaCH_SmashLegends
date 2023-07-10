using UnityEngine;

public class LegendAnimationController : MonoBehaviour
{
    //추후 리소스 할당
    [SerializeField]
    private AnimationClip[] _applyAttackClip;
    [SerializeField]
    private AnimationClip[] _applyJumpAttackClip;
    private AnimationClip[] _applyAnimationClip;

    private LegendController _legendController;
    private Animator _animator;
    private AnimatorOverrideController _animatorOverrideController;
    
    private string[] _overrideAnimatorName;
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
    public void PlayNextAnimation(ActionType actionType)
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
    public void PlayNextAnimationClip(ComboAttackType type)
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
    public void ResetComboAttackAnimationClip()
    {
        _animationClipIndex = 0;
        _animatorOverrideController[StringLiteral.AnimationClipLiteral[0]] = _applyAttackClip[0];
        _animatorOverrideController[StringLiteral.JumpAnimationClipLiteral] = _applyJumpAttackClip[0];
    }
}

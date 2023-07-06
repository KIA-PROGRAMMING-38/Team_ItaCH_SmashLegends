using UnityEngine;
using UnityEngine.InputSystem;

public enum ActionType
{
    Move = 0,
    Jump = 1,
    DefaultAttack = 2,
    HeavyAttack = 3,
    SkillAttack = 4
}

public class LegendController : MonoBehaviour
{
    public Vector3 MoveDirection { get; private set; }

    private Animator _anim;
    private InputAction[] _action;
    private string[] _actionLiteral = new[] { "Move", "Jump", "DefaultAttack", "SmashAttack", "SkillAttack" };

    // 추후 스트링 리터럴 캐싱 후 사용
    private string[] _animationClipLiteral = new[] { "Peter_FirstAttack", "Peter_SecondAttack" };

    private ActionType _actionType;
    private bool _isAttack { get; set; }
    public int PossibleComboCount { get; set; } = 0;
    public int AnimationClipIndex { get; set; } = 0;

    private AnimatorOverrideController _animatorOverride;
    private UnityEngine.InputSystem.PlayerInput _input;
    private CharacterStatus _characterStatus;

    // 추후 리소스로 매니저로 캐싱 후 사용
    // Character Type 으로 변환해야함.
    private AnimatorOverrideController hook;

    private string[] _overrideAnimatorName;
    private AnimationClip[] _applyAnimationClip;
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _input = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        _characterStatus = GetComponent<CharacterStatus>();
        _action = new InputAction[_actionLiteral.Length];
        hook = Resources.Load<AnimatorOverrideController>(ResourcesManager.HookAnimator);

        _overrideAnimatorName = new string[_anim.runtimeAnimatorController.animationClips.Length];
        _applyAnimationClip = new AnimationClip[_anim.runtimeAnimatorController.animationClips.Length];
        _animatorOverride = new AnimatorOverrideController(_anim.runtimeAnimatorController);

        for (int i = 0; i < _anim.runtimeAnimatorController.animationClips.Length; ++i)
        {

            SetAnimatorClip(i);
        }

        _anim.runtimeAnimatorController = _animatorOverride;
    }

    private void Start()
    {
        for (int i = 0; i < _action.Length; ++i)
        {
            _action[i] = _input.actions[_actionLiteral[i]];
        }
    }
    private void Update()
    {
    }
    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        if (input != null)
        {
            MoveDirection = new Vector3(input.x, 0, input.y);
            _anim.SetBool(AnimationHash.Run, true);
        }
    }
    private void OnJump() { }
    private void OnDefaultAttack() { }
    private void OnSmashAttack() { }
    private void OnSkillAttack() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HangZone"))
        {
            // Animation.Play(Hang);
        }
    }
    public void SetNextAnimation()
    {
        if (_action[(int)ActionType.Move].triggered)
        {
            _anim.Play(AnimationHash.Run);
            return;
        }
        if (_action[(int)ActionType.Jump].triggered)
        {
            _anim.Play(AnimationHash.Jump);
            return;
        }
        if (_action[(int)ActionType.DefaultAttack].triggered)
        {
            _anim.Play(AnimationHash.FirstAttack);
            return;
        }
        if (_action[(int)ActionType.HeavyAttack].triggered)
        {
            _anim.Play(AnimationHash.HeavyAttack);
            return;
        }
        if (_action[(int)ActionType.SkillAttack].triggered)
        {
            _anim.Play(AnimationHash.SkillAttack);
            return;
        }
    }
    public void PlayFirstAttack()
    {
        if (SetAttackable())
        {
            _anim.Play(AnimationHash.FirstAttack);
            SetRotateOnAttack();
        }
    }
    public void PlaySecondAttack()
    {
        if (SetAttackable())
        {
            _anim.Play(AnimationHash.SecondAttack);
            SetRotateOnAttack();
        }
    }
    private void SetAnimatorClip(int i)
    {
        _overrideAnimatorName[i] = _animatorOverride.animationClips[i].name;
        //hook.animationClips => 추후 캐릭터 Type 으로
        _applyAnimationClip[i] = hook.animationClips[i];
        _animatorOverride[_overrideAnimatorName[i]] = _applyAnimationClip[i];
    }
    public void PlayNextClip()
    {
        //if (PossibleComboCount < ApplyClip.Length - 1)
        //{
        //    _animatorOverride[_animationClipLiteral[AnimationClipIndex]] = ApplyClip[PossibleComboCount];
        //    ++PossibleComboCount;
        //    ++AnimationClipIndex;
        //    if (AnimationClipIndex == 2)
        //    {
        //        AnimationClipIndex = 0;
        //    }
        //}
    }
    public bool SetAttackable()
    {
        if (_isAttack)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void SetComboPossible() => _isAttack = true;
    public void SetComboImPossible() => _isAttack = false;
    private void SetRotateOnAttack()
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

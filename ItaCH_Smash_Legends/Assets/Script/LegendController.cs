using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class LegendController : MonoBehaviour
{
    // 추후 리소스로 매니저로 캐싱 후 사용
    public AnimationClip[] ApplyClip;
    public Animator peter;

    public Vector3 MoveDirection { get; private set; }
    private Animator _anim;
    private InputAction[] _action = new InputAction[5];
    private string[] _actionLiteral = new[] { "Move", "Jump", "DefaultAttack", "SmashAttack", "SkillAttack" };
    
    // 추후 스트링 리터럴 캐싱 후 사용
    private string[] _animationClipLiteral = new[]{"Hook_FirstAttack", "Hook_FinishAttack" };

    public int ActionMove { get; private set; } = 0;
    public int ActionJump { get; private set; } = 1;
    public int ActionDefaultAttack { get; private set; } = 2;
    public int ActionHeavyAttack { get; private set; } = 3;
    public int ActionSkillAttack { get; private set; } = 4;

    private AnimatorOverrideController _animatorOverride;
    public int PossibleComboCount { get; set; } = 0;

    private UnityEngine.InputSystem.PlayerInput _input;

    private void Awake()
    {
        _anim = GetComponent<Animator>();

        // 애니메이션 교체 가능 확인완료
        //_anim.runtimeAnimatorController = Instantiate(Resources.Load<RuntimeAnimatorController>(ResourcesManager.PeterAnimator));
        
        _input = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        _animatorOverride = new AnimatorOverrideController(_anim.runtimeAnimatorController);
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

    public void NextAnimation()
    {
        if (OnActionTrigger(ActionJump))
        {
            _anim.Play(AnimationHash.Jump);
        }
        if (OnActionTrigger(ActionDefaultAttack))
        {
            _anim.Play(AnimationHash.FirstAttack);
        }
    }
    public void PlayFirstAttack()
    {
        if (SetAttackable(ActionDefaultAttack))
        {
            _anim.Play(AnimationHash.FirstAttack);
        }
    }
    public void PlaySecondAttack()
    {
        if (SetAttackable(ActionDefaultAttack))
        {
            _anim.Play(AnimationHash.SecondAttack);
        }
    }
    public void NextPlayClip()
    {
        if (PossibleComboCount < ApplyClip.Length - 1)
        {
            _animatorOverride[_animationClipLiteral[PossibleComboCount]] = ApplyClip[PossibleComboCount];
            ++PossibleComboCount;
        }
    }

    public bool OnActionTrigger(int actionNumber)
    {
        if (_action[actionNumber].triggered)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool SetAttackable(int actionNumber)
    {
        if(OnActionTrigger(actionNumber) && PossibleComboCount < ApplyClip.Length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // PlayerRollUp 
    // DownIdle 상태에서 기능 수행

    // PlayerHangController
    // Hang 상태에서 입력시 처리
}

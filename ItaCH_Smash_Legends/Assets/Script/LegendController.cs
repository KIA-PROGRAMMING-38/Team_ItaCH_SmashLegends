using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class LegendController : MonoBehaviour
{
    public Vector3 _moveDirection;
    private Animator _anim;
    private InputAction _jump;
    private Avatar _avatar;
    UnityEngine.InputSystem.PlayerInput _input;

    private void Awake()
    {
        // 애니메이션 교체
        _anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(ResourcesManager.PeterDefaultAttackIcon);
    }
    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        if (input != null)
        {
            _anim.Play("Run");
            _moveDirection = new Vector3(input.x, 0, input.y);
        }
    }

    // 애니메이터 오버라이드 -> 1.점프공격 3회 캐릭터 -> 1회로 바꾸는 형태
    // 2.노멀라이즈 시간으로 애니메이션 클립재생 ?
    // 3.if(AnimationClip[CurrentPossibleComboCount] != null) 콤보카운트를 애니메이션 이벤트로 감소
    // 다음 애니메이션 재생 

    private void OnJump()
    {
        //_jump = _input.actions["Jump"];
        //if (_jump.triggered)
        //{
        //    Debug.Log(_jump);
        //}

        // 1.Anumation.SetBool() => 런, 아이들, 다운상태, 행 상태에서 가능하게
        // 2.각 상태에서 Jump.triggered 로 전환가능

        // 로직은 JumpState 에서 
    }

    private void OnDefaultAttack()
    {
        //1. Animation.SetBool(DefaultAttack,true); => 런 , 아이들 때만 넘어갈 수 있음
        //2. Run, Idle 스테이트 머신 에서 triggered 시 전환 가능

        // 애니메이션 이벤트로 공격 가능 횟수 -1 
        // 해당 횟수에서 DefaultAttack.triggered 라면 다음 애니메이션 재생?
    }

    private void OnHeavyAttack()
    {
        //1. Animation.SetBool(HeavyAttack,true); => 런 , 아이들 때만 넘어갈 수 있음
        //2. Run, Idle 스테이트 머신 에서 triggered 시 전환 가능
    }

    private void OnSKillAttack()
    {
        //1. Animation.SetBool(SkillAttack,true); => 런 , 아이들 때만 넘어갈 수 있음
        //2. Run, Idle 스테이트 머신 에서 triggered 시 전환 가능
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Edge"))
        {
            // Animation.Play(Hang);
        }

    }

    // PlayerRollUp 
    // DownIdle 상태에서 기능 수행

    // PlayerHangController
    // Hang 상태에서 입력시 처리
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerAttack _attack;
    private PlayerJump _jump;
    private Animator _animator;

    public bool IsCombo { get; private set; }
    public bool IsSmash { get; private set; }

    private void Awake()
    {
        _attack = GetComponent<PlayerAttack>();
        _jump = GetComponent<PlayerJump>();
        _animator = GetComponent<Animator>();
        
    }
    // 인풋시스템 이벤트 
    private void OnDefaultAttack()
    {
        // 스테이트 콤보 시작 전환
        IsCombo = true;
        // 어택에 따른 소폭 전진
        _attack.AttackOnDefaultDash();

        // 조건에 따라 애니메이션 진입 점 설정
        if(_attack.CurrentPossibleComboCount == _attack.COMBO_FINISH_COUNT)
        {
            
        }
        else if(_attack.CurrentPossibleComboCount == _attack.COMBO_SECOND_COUNT)
        {

        }
        else
        {

        }
    }

    // 이벤트 호출
    private void OnJump()
    {
        _jump.JumpInput();
    }

   
}
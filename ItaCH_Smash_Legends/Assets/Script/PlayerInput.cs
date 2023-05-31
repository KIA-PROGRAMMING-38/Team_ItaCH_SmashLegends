using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerAttack _attack;

    public bool IsCombo { get; private set; }
    public bool IsSmash { get; private set; }

    private void Awake()
    {
        _attack = GetComponent<PlayerAttack>();
    }
    // 인풋시스템 이벤트 
    private void OnDefaultAttack()
    {
        // 스테이트 콤보 시작 전환
        IsCombo = true;
        // 어택에 따른 소폭 전진
        _attack.AttackOnFirstDash();

        // 조건에 따라 animator.Play 방식 설정해야함.
    }

   
}
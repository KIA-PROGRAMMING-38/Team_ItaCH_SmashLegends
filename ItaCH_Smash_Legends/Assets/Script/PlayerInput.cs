using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerAttack _attack;
    private Animator _animator;

    public bool IsCombo { get; private set; }
    public bool IsSmash { get; private set; }

    private void Awake()
    {
        _attack = GetComponent<PlayerAttack>();
        _animator = GetComponent<Animator>();
        
    }
    // ��ǲ�ý��� �̺�Ʈ 
    private void OnDefaultAttack()
    {
        // ������Ʈ �޺� ���� ��ȯ
        IsCombo = true;
        // ���ÿ� ���� ���� ����
        _attack.AttackOnDefaultDash();

        // ���ǿ� ���� �ִϸ��̼� ���� �� ����
        if(_attack.CurrentPossibleComboCount == _attack.COMBO_FINISH_COUNT)
        {
            
        }
        else if(_attack.CurrentPossibleComboCount == _attack.COMBO_SECOND_COUNT)
        {

        }
        //1Ÿ
        else
        {

        }


    }

   
}
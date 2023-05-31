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
    // ��ǲ�ý��� �̺�Ʈ 
    private void OnDefaultAttack()
    {
        // ������Ʈ �޺� ���� ��ȯ
        IsCombo = true;
        // ���ÿ� ���� ���� ����
        _attack.AttackOnFirstDash();

        // ���ǿ� ���� animator.Play ��� �����ؾ���.
    }

   
}
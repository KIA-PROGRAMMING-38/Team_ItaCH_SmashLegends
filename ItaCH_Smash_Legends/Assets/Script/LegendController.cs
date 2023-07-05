using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LegendController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
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
        // �ִϸ��̼� ��ü
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

    // �ִϸ����� �������̵� -> 1.�������� 3ȸ ĳ���� -> 1ȸ�� �ٲٴ� ����
    // 2.��ֶ����� �ð����� �ִϸ��̼� Ŭ����� ?
    // 3.if(AnimationClip[CurrentPossibleComboCount] != null) �޺�ī��Ʈ�� �ִϸ��̼� �̺�Ʈ�� ����
    // ���� �ִϸ��̼� ��� 

    private void OnJump()
    {
        //_jump = _input.actions["Jump"];
        //if (_jump.triggered)
        //{
        //    Debug.Log(_jump);
        //}

        // 1.Anumation.SetBool() => ��, ���̵�, �ٿ����, �� ���¿��� �����ϰ�
        // 2.�� ���¿��� Jump.triggered �� ��ȯ����

        // ������ JumpState ���� 
    }

    private void OnDefaultAttack()
    {
        //1. Animation.SetBool(DefaultAttack,true); => �� , ���̵� ���� �Ѿ �� ����
        //2. Run, Idle ������Ʈ �ӽ� ���� triggered �� ��ȯ ����

        // �ִϸ��̼� �̺�Ʈ�� ���� ���� Ƚ�� -1 
        // �ش� Ƚ������ DefaultAttack.triggered ��� ���� �ִϸ��̼� ���?
    }

    private void OnHeavyAttack()
    {
        //1. Animation.SetBool(HeavyAttack,true); => �� , ���̵� ���� �Ѿ �� ����
        //2. Run, Idle ������Ʈ �ӽ� ���� triggered �� ��ȯ ����
    }

    private void OnSKillAttack()
    {
        //1. Animation.SetBool(SkillAttack,true); => �� , ���̵� ���� �Ѿ �� ����
        //2. Run, Idle ������Ʈ �ӽ� ���� triggered �� ��ȯ ����
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Edge"))
        {
            // Animation.Play(Hang);
        }

    }

    // PlayerRollUp 
    // DownIdle ���¿��� ��� ����

    // PlayerHangController
    // Hang ���¿��� �Է½� ó��
}

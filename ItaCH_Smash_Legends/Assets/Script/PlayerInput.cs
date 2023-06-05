using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerAttack _playerAttack;
    private PlayerJump _playerJump;
    private PlayerMove _playerMove;
    private PlayerStatus _playerStatus;
    private Animator _animator;
    public bool IsCombo { get; private set; }
    public bool IsSmash { get; private set; }


    private void Awake()
    {

        _playerMove = GetComponent<PlayerMove>();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerJump = GetComponent<PlayerJump>();
        _playerStatus = GetComponent<PlayerStatus>();

        _animator = GetComponent<Animator>();
        IsCombo = false;
    }

    private void OnDefaultAttack()
    {

        if (_playerStatus.IsJump == false)
        {
            _animator.SetBool(AnimationHash.Jump, false);
            //_animator.SetTrigger(AnimationHash.JumpAttack);
            _animator.Play(AnimationHash.JumpAttack);
            return;
        }


        if (_playerAttack.CurrentPossibleComboCount == _playerAttack.MAX_POSSIBLE_ATTACK_COUNT)
        {
            _animator.SetBool(AnimationHash.FirstAttack, true);
            _playerAttack.isAttack = true;
        }

        if (_playerAttack.isFirstAttack && _playerAttack.CurrentPossibleComboCount == _playerAttack.COMBO_SECOND_COUNT)
        {
            _playerAttack.isSecondAttack = true;
        }

        if (_playerAttack.isSecondAttack && _playerAttack.CurrentPossibleComboCount == _playerAttack.COMBO_FINISH_COUNT)
        {
            _playerAttack.isFinishAttack = true;
        }
    }

    private void OnSmashAttack()
    {
        if (_playerStatus.CurrentState == PlayerStatus.State.Run || 
            _playerStatus.CurrentState == PlayerStatus.State.Idle)
        {
            _animator.Play(AnimationHash.HeavyAttack);
            _playerStatus.CurrentState = PlayerStatus.State.HeavyAttack;
       
        }
    }
    private void OnJump()
    {
        if (_playerStatus.IsJump)
        {
            _playerJump.JumpInput();
            _animator.SetBool(AnimationHash.Jump, true);
        }

        if(_playerStatus.IsHang)
        {
            _playerJump.JumpInput();
            _playerStatus.IsHang = false;

            // 점프 애니메이션 변경으로 바꿀것 
            _animator.Play(AnimationHash.Idle);
        }

    }

    private void OnMove(InputValue value)
    {
        if (_playerStatus.IsHang == false)
        {
            _playerMove.MoveHellper(value);
        }
    }

}
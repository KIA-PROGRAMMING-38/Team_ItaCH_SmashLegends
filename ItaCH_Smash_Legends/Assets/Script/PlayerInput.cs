using Cysharp.Threading.Tasks;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerAttack _playerAttack;
    private PlayerJump _playerJump;
    private PlayerMove _playerMove;
    private PlayerStatus _playerStatus;
    private PlayerHit _playerHit;
    private PlayerRollUp _playerRollUp;
    private Animator _animator;


    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerJump = GetComponent<PlayerJump>();
        _playerStatus = GetComponent<PlayerStatus>();
        _playerHit= GetComponent<PlayerHit>();
        _playerRollUp = GetComponent<PlayerRollUp>();
        _animator = GetComponent<Animator>();
    }

    private void OnDefaultAttack()
    {

        if (_playerStatus.IsJump == false)
        {
            _animator.SetTrigger(AnimationHash.JumpAttack);
            return;
        }

        if (IsPossibleFirstAttack())
        {
            _animator.Play(AnimationHash.FirstAttack);
            _playerHit.AttackRangeOn();
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
    private bool IsPossibleFirstAttack()
    {
        if (_playerAttack.CurrentPossibleComboCount == _playerAttack.MAX_POSSIBLE_ATTACK_COUNT &&
         _playerStatus.CurrentState == PlayerStatus.State.Run ||
         _playerStatus.CurrentState == PlayerStatus.State.Idle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnSmashAttack()
    {
        if (_playerStatus.CurrentState == PlayerStatus.State.Run ||
            _playerStatus.CurrentState == PlayerStatus.State.Idle)
        {
            _animator.Play(AnimationHash.HeavyAttack);
            _playerStatus.CurrentState = PlayerStatus.State.HeavyAttack;
            _playerHit.AttackRangeOn();
        }
    }

    private void OnJump()
    {
        if (_playerStatus.IsJump && _playerStatus.CurrentState == PlayerStatus.State.Run
            || _playerStatus.CurrentState == PlayerStatus.State.Idle)
        {
            _playerJump.JumpInput();
            _animator.Play(AnimationHash.Jump);
        }

        if (_playerStatus.IsHang)
        {
            _playerJump.JumpInput();
            _animator.Play(AnimationHash.Jump);
            _playerStatus.IsHang = false;

        }

    }

    private void OnMove(InputValue value)
    {
        if (_playerStatus.IsHang == false)
        {
            _playerMove.MoveHellper(value);
        }

        if (_playerStatus.IsRollUp)
        {
            _playerMove.MoveHellper(value);
            _playerRollUp.RollingDirection();
        }
    }
}
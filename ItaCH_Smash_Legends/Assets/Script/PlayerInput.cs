using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInput : MonoBehaviour
{
    private PlayerAttack _playerAttack;
    private PlayerJump _playerJump;
    private PlayerMove _playerMove;
    private PlayerStatus _playerStatus;
    private PlayerRollUp _playerRollUp;
    private Animator _animator;
    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerJump = GetComponent<PlayerJump>();
        _playerStatus = GetComponent<PlayerStatus>();
        _playerRollUp = GetComponent<PlayerRollUp>();
        _animator = GetComponent<Animator>();
    }
    private void OnDefaultAttack()
    {
        _playerAttack.JumpAttack();
        _playerAttack.DefaultAttack();
    }
    private void OnSmashAttack()
    {
        _playerAttack.HeavyAttack();
    }
    private void OnSkillAttack()
    {
        _playerAttack.SkillAttack();
    }
    private void OnJump()
    {
        if (_playerStatus.IsJump)
        {
            if(_playerStatus.CurrentState == PlayerStatus.State.Run
            || _playerStatus.CurrentState == PlayerStatus.State.Idle
            || _playerStatus.CurrentState == PlayerStatus.State.HitUp)
            {
                _playerJump.JumpInput();
                _animator.Play(AnimationHash.Jump);
            }
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
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerJump _playerJump;
    private PlayerMove _playerMove;
    private PlayerAttack _playerAttack;
    private Animator _animator;

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerJump = GetComponent<PlayerJump>();
        _playerAttack = GetComponent<PlayerAttack>();
        _animator = GetComponent<Animator>();

    }

    public void OnDefaultAttack()
    {
        if (_playerAttack.CurrentPossibleComboCount == _playerAttack.MAX_POSSIBLE_ATTACK_COUNT)
        {
            _animator.SetBool(AnimationHash.FirstAttack, true);
            _playerAttack.isAttack = true;
        }

        if(_playerAttack.CurrentPossibleComboCount == _playerAttack.COMBO_SECOND_COUNT && _playerAttack.isFirstAttack)
        {
            _playerAttack.isSecondAttack = true;
        }

        if(_playerAttack.CurrentPossibleComboCount == _playerAttack.COMBO_FINISH_COUNT)
        {
            _playerAttack.isFinishAttack = true;
        }
    }

    private void OnJump()
    {
        _playerJump.JumpInput();
    }

    private void OnMove(InputValue value)
    {
        _playerMove.MoveHellper(value);
    }

}
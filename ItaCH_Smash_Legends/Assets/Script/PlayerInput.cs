using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerJump _playerJump;
    private PlayerMove _playerMove;
    private PlayerAttack _playerAttack;
    private Animator _animator;

    internal bool isFirstAttack;
    internal bool isSecondAttack;

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
        }

        if(_playerAttack.CurrentPossibleComboCount == _playerAttack.COMBO_SECOND_COUNT && isFirstAttack)
        {
            isSecondAttack = true;
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
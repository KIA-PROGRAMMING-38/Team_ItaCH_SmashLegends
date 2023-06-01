using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerAttack _playerAttack;
    private PlayerJump _playerJump;
    private PlayerMove _playerMove;
    private Animator _animator;

    public bool IsCombo { get; private set; }
    public bool IsSmash { get; private set; }

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerJump = GetComponent<PlayerJump>();
        _animator = GetComponent<Animator>();
        IsCombo = false;
    }

    private void OnDefaultAttack()
    {
        IsCombo = true;

        if (_playerAttack.CurrentPossibleComboCount == _playerAttack.COMBO_FINISH_COUNT)
        {
        }
        else if (_playerAttack.CurrentPossibleComboCount == _playerAttack.COMBO_SECOND_COUNT)
        {

        }
        else
        {

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
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private CharacterStatus _characterStatus;
    private PlayerStatus _playerStatus;
    private Animator _animator;

    internal Vector3 moveDirection;
    internal float _currentMoveSpeed = 5.4f;

    private void Awake()
    {
        _characterStatus= GetComponent<CharacterStatus>();
        _playerStatus = GetComponent<PlayerStatus>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _currentMoveSpeed = _characterStatus.MoveSpeed;
    }

    public void MoveAndRotate()
    {
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
            transform.Translate(Vector3.forward * (_currentMoveSpeed * Time.deltaTime));
        }
        else
        {
            _animator.SetBool(AnimationHash.Run, false);
        }
    }

    public void MoveHellper(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        if (input != null)
        {
            moveDirection = new Vector3(input.x, 0, input.y);
            if (_playerStatus.CurrentState == PlayerStatus.State.Idle
                || _playerStatus.CurrentState == PlayerStatus.State.Run)
            {
                _animator.SetBool(AnimationHash.Run, true);
            }

        }
    }
}
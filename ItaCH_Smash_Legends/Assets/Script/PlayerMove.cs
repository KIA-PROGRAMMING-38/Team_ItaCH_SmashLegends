using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _currentMoveSpeed;

    private PlayerStatus _playerStatus;
    internal Vector3 moveDirection;
    private Animator _animator;
    private PlayerStatus _playerStatue;
    private void Start()
    {
        _playerStatue = GetComponent<PlayerStatus>();
        _animator = GetComponent<Animator>();
        _playerStatus = GetComponent<PlayerStatus>();
    }

    // 이동방향을 바라보도록 이동방향으로 캐릭터 회전
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
            _playerStatus.CurrentState = PlayerStatus.State.Idle;
        }
    }

    public void MoveHellper(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        if (input != null)
        {
            moveDirection = new Vector3(input.x, 0, input.y);

            if (_playerStatus.CurrentState == PlayerStatus.State.Idle)
            {
                _animator.SetBool(AnimationHash.Run, true);
            }
        }
    }
}
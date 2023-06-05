using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _currentMoveSpeed;

    internal Vector3 moveDirection;
    private Animator _animator;
    private PlayerStatus _playerStatue;
    private void Start()
    {
        _playerStatue = GetComponent<PlayerStatus>();
        _animator = GetComponent<Animator>();
    }

    // �̵������� �ٶ󺸵��� �̵��������� ĳ���� ȸ��
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
            _animator.SetBool(AnimationHash.Run, true);
        }
    }
}
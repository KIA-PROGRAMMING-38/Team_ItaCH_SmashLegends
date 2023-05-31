using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _currentMoveSpeed;

    private Vector3 _moveDirection;
    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }
    private void Update()
    {
        MoveAndRotate();
    }

    // �̵������� �ٶ󺸵��� �̵��������� ĳ���� ȸ��
    private void MoveAndRotate()
    {
        if (_moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(_moveDirection);
            transform.Translate(Vector3.forward * (_currentMoveSpeed * Time.deltaTime));
            _anim.Play(AnimationHash.Run);
        }
        else
        {
            _anim.Play(AnimationHash.Idle);
        }
    }

    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        if (input != null)
        {
            _moveDirection = new Vector3(input.x, 0, input.y);
        }
    }
}
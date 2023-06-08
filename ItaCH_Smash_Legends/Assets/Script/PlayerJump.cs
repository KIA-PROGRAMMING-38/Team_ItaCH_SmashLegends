using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{

    private PlayerStatus _playerStatus;
    private PlayerMove _playerMove;

    internal Rigidbody _rigidbody;
    private Animator _animator;

    private float _jumpAcceleration = 14.28f; // 점프 가속도
    private float _maxFallingSpeed = 23f; // 최대 낙하 속도
    private float _gravitationalAcceleration = 36f; // 중력 가속도

    public static readonly float MAX_JUMP_POWER = 1f;
    private static readonly Vector3 JUMP_DIRECTION = Vector3.up;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerStatus = GetComponent<PlayerStatus>();
        _playerMove = GetComponent<PlayerMove>();
        _animator = GetComponent<Animator>();

        // 중력 현재 피터 기준으로 설정. 중력은 아래로 적용되어야 하니 음수값이 적용되어야 하므로 - 붙여놓음
        Physics.gravity = new Vector3(0f, -_gravitationalAcceleration, 0f);
    }

    void Start()
    {
        // 플레이어의 mass 설정
        _rigidbody.mass = MAX_JUMP_POWER / _jumpAcceleration;
    }

    private void FixedUpdate()
    {
        FixedMaxFallSpeed();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && _playerStatus.IsJump == false)
        {
            _playerStatus.IsJump = true;
            _animator.SetBool(AnimationHash.JumpDown, false);

        }
    }

    public void JumpMoveAndRotate()
    {
        if (_playerMove.moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(_playerMove.moveDirection);
            transform.Translate(Vector3.forward * (_playerMove._currentMoveSpeed * Time.deltaTime));

        }
    }

    private void FixedMaxFallSpeed()
    {
        if (_rigidbody.velocity.y <= 0f)
        {
            Vector3 currentVelocity = _rigidbody.velocity;

            if (currentVelocity.magnitude > _maxFallingSpeed)
            {
                currentVelocity = currentVelocity * _maxFallingSpeed / currentVelocity.magnitude;
                _rigidbody.velocity = currentVelocity;
            }
        }
    }


    public void JumpInput()
    {
        _rigidbody.AddForce(JUMP_DIRECTION * MAX_JUMP_POWER, ForceMode.Impulse);
    }
}

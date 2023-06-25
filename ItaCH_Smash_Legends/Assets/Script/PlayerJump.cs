using UnityEngine;

public class PlayerJump : MonoBehaviour
{

    private PlayerStatus _playerStatus;
    private PlayerMove _playerMove;

    internal Rigidbody _rigidbody;
    private Animator _animator;

    private float _jumpAcceleration = 14.28f;
    private float _maxFallingSpeed = 23f;
    private float _gravitationalAcceleration = 36f;

    public static readonly float MAX_JUMP_POWER = 1f;
    private static readonly Vector3 JUMP_DIRECTION = Vector3.up;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerStatus = GetComponent<PlayerStatus>();
        _playerMove = GetComponent<PlayerMove>();
        _animator = GetComponent<Animator>();

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
            _animator.SetBool(AnimationHash.JumpDown, false);
            _playerStatus.IsJump = true;
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
        if (_playerStatus.CurrentState == PlayerStatus.State.HitUp)
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }
}

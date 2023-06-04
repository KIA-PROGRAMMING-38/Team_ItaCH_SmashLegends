using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    internal Rigidbody _rigidbody;
    private Animator _animator;

    [SerializeField] private float _jumpAcceleration; // 점프 가속도
    [SerializeField] private float _maxFallingSpeed; // 최대 낙하 속도
    [SerializeField] private float _gravitationalAcceleration; // 중력 가속도

    public static readonly float MAX_JUMP_POWER = 1f;
    private static readonly Vector3 JUMP_DIRECTION = Vector3.up;

    internal bool isJump = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
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
        if (collision.gameObject.CompareTag("Ground") && !isJump)
        {
            _animator.SetBool(AnimationHash.JumpDown, false);
            isJump = true;
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
        if (isJump)
        {
            isJump = false;
            _rigidbody.AddForce(JUMP_DIRECTION * MAX_JUMP_POWER, ForceMode.Impulse);
        }
    }
}

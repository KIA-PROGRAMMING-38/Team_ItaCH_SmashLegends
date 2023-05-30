using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [SerializeField] private float _jumpAcceleration; // 점프 가속도
    [SerializeField] private float _maxFallingSpeed; // 최대 낙하 속도
    [SerializeField] private float _gravitationalAcceleration; // 중력 가속도

    public static readonly float MAX_JUMP_POWER = 1f;
    private static readonly Vector3 JUMP_DIRECTION = Vector3.up;

    private bool _isJump = false;
    private bool _isFall = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

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

    void Update()
    {
        JumpUp();
    }


    private void OnCollisionEnter(Collision collision)
    {
        // 2단 점프를 막기 위한 코드
        // 땅으로 사용할 오브젝트에 Ground 태그 붙여줄 것
        if (collision.gameObject.CompareTag("Ground") && _isFall)
        {
            PlayJumpLanding();
        }
    }

    private void PlayJumpLanding()
    {
        // "JumpLanding" 애니메이션 재생

        _isJump = false;
        _isFall = false;
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

    private void JumpUp()
    {
        if (_rigidbody.velocity.y < 0f && _isFall == false && _isJump)
        {
            _isFall = true;

            // "JumpDown" 애니메이션 재생
        }
    }

    private void OnJump()
    {
        if (_isJump == false)
        {
            _isJump = true;

            _rigidbody.AddForce(JUMP_DIRECTION * MAX_JUMP_POWER, ForceMode.Impulse);

            // "JumpUp" 애니메이션 재생
        }
    }
}

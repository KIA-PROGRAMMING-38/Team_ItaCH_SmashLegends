using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [SerializeField] private float _jumpAcceleration; // ���� ���ӵ�
    [SerializeField] private float _maxFallingSpeed; // �ִ� ���� �ӵ�
    [SerializeField] private float _gravitationalAcceleration; // �߷� ���ӵ�

    public static readonly float MAX_JUMP_POWER = 1f;
    private static readonly Vector3 JUMP_DIRECTION = Vector3.up;

    private bool _isJump = false;
    private bool _isFall = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        // �߷� ���� ���� �������� ����. �߷��� �Ʒ��� ����Ǿ�� �ϴ� �������� ����Ǿ�� �ϹǷ� - �ٿ�����
        Physics.gravity = new Vector3(0f, -_gravitationalAcceleration, 0f);
    }
    void Start()
    {
        // �÷��̾��� mass ����
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
        // 2�� ������ ���� ���� �ڵ�
        // ������ ����� ������Ʈ�� Ground �±� �ٿ��� ��
        if (collision.gameObject.CompareTag("Ground") && _isFall)
        {
            PlayJumpLanding();
        }
    }

    private void PlayJumpLanding()
    {
        // "JumpLanding" �ִϸ��̼� ���

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

            // "JumpDown" �ִϸ��̼� ���
        }
    }

    private void OnJump()
    {
        if (_isJump == false)
        {
            _isJump = true;

            _rigidbody.AddForce(JUMP_DIRECTION * MAX_JUMP_POWER, ForceMode.Impulse);

            // "JumpUp" �ִϸ��̼� ���
        }
    }
}

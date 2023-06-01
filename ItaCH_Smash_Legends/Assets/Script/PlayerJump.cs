using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [SerializeField] private float _jumpAcceleration; // ���� ���ӵ�
    [SerializeField] private float _maxFallingSpeed; // �ִ� ���� �ӵ�
    [SerializeField] private float _gravitationalAcceleration; // �߷� ���ӵ�

    public static readonly float MAX_JUMP_POWER = 1f;
    private static readonly Vector3 JUMP_DIRECTION = Vector3.up;

    private bool _isJump = false;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isJump = true;
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
        if (_isJump)
        {
            _rigidbody.AddForce(JUMP_DIRECTION * MAX_JUMP_POWER, ForceMode.Impulse);
            _isJump = false;
        }
    }
}

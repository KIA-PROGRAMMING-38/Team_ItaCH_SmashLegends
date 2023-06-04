using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    internal Rigidbody _rigidbody;
    private Animator _animator;

    [SerializeField] private float _jumpAcceleration; // ���� ���ӵ�
    [SerializeField] private float _maxFallingSpeed; // �ִ� ���� �ӵ�
    [SerializeField] private float _gravitationalAcceleration; // �߷� ���ӵ�

    public static readonly float MAX_JUMP_POWER = 1f;
    private static readonly Vector3 JUMP_DIRECTION = Vector3.up;

    internal bool isJump = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

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

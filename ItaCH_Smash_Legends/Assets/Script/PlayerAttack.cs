using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public Collider Weapon;

    private const int MAX_POSSIBLE_ATTACK_COUNT = 3;
    private int _currnetPossibleComboCount = MAX_POSSIBLE_ATTACK_COUNT;
    private float _defaultDashPower = 1.7f;
    private float _lightKnockbackPower = 5f;
    private float _heavyKnockbackPower = 20f;
    private bool _isHit;
    private Vector3 _knockbackDirection;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
        _knockbackDirection = transform.forward + transform.up;
    }
    void Update()
    {
        // �˹� 1ȸ�� �����ϰ�.
        Weapon.enabled = true;
        _isHit = true;

        // ����

    }

    public void AttackOnFirstDash()
    {
        // ���� ����
        _rigidbody.AddForce(transform.forward * _defaultDashPower, ForceMode.Impulse);
        // ����Ʈ�� �ִϸ��̼� �̺�Ʈ�� �����ؾ���.
    }

    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾�� ���Ⱑ �浹������ �˹� ó��
        if (other.CompareTag("Player"))
        {
            //���� ���� ����
            Weapon.enabled = false;

            Rigidbody rigidbody = other.GetComponent<Rigidbody>();
            if (_isHit)
            {

                if (_currnetPossibleComboCount == 0)
                {
                    rigidbody.AddForce(_knockbackDirection * _heavyKnockbackPower, ForceMode.Impulse);
                }
                else
                {
                    rigidbody.AddForce(_knockbackDirection * _lightKnockbackPower, ForceMode.Impulse);
                }
                // ���� ���� ����x
                _isHit = false;
            }
        }
    }
}

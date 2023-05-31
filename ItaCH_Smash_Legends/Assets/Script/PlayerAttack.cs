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
        // 넉백 1회만 가능하게.
        Weapon.enabled = true;
        _isHit = true;

        // 조건

    }

    public void AttackOnFirstDash()
    {
        // 소폭 전진
        _rigidbody.AddForce(transform.forward * _defaultDashPower, ForceMode.Impulse);
        // 이펙트는 애니메이션 이벤트로 설정해야함.
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어와 무기가 충돌했을때 넉백 처리
        if (other.CompareTag("Player"))
        {
            //연속 엔터 방지
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
                // 연속 엔터 방지x
                _isHit = false;
            }
        }
    }
}

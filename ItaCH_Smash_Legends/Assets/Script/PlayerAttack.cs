using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public Collider Weapon;

    public readonly int MAX_POSSIBLE_ATTACK_COUNT = 3;
    public readonly int COMBO_SECOND_COUNT = 2;
    public readonly int COMBO_FINISH_COUNT = 1;

    public int CurrentPossibleComboCount;

    private float _defaultDashPower = 1.7f;
    private float _smashDashPower = 3.4f;

    private float _lightKnockbackPower = 5f;
    private float _heavyKnockbackPower = 20f;
    private bool _isHit;
    private Vector3 _knockbackDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _knockbackDirection = transform.forward + transform.up;
        CurrentPossibleComboCount = MAX_POSSIBLE_ATTACK_COUNT;
    }
    void Update()
    {
        // 추후 맞는캐릭터 피격 애니메이션 재생동안 안맞게끔 수정.
        Weapon.enabled = true;
        _isHit = true;
    }

    public void AttackOnDefaultDash()
    {
        if (CurrentPossibleComboCount == COMBO_FINISH_COUNT)
        {
            _rigidbody.AddForce(transform.forward * _smashDashPower, ForceMode.Impulse);
        }
        else
        {
            _rigidbody.AddForce(transform.forward * _defaultDashPower, ForceMode.Impulse);
        }
    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //연속 엔터 방지
            Weapon.enabled = false;

            Rigidbody rigidbody = other.GetComponent<Rigidbody>();
            if (_isHit)
            {
                if (CurrentPossibleComboCount == COMBO_FINISH_COUNT)
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

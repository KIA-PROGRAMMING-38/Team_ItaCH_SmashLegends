using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public readonly int MAX_POSSIBLE_ATTACK_COUNT = 3;
    public readonly int COMBO_SECOND_COUNT = 2;
    public readonly int COMBO_FINISH_COUNT = 1;
    public int CurrentPossibleComboCount;

    internal bool isFirstAttack;
    internal bool isSecondAttack;
    internal bool isFinishAttack;

    private PlayerMove _playerMove;
    private Rigidbody _rigidbody;
    public Collider Weapon;

    private Vector3 _knockbackDirection;

    private bool _isHit;
    
    private float _defaultDashPower = 1f;
    private float _lightKnockbackPower = 5f;
    private float _heavyKnockbackPower = 20f;




    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        _rigidbody = GetComponent<Rigidbody>();
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
        _rigidbody.AddForce(transform.forward * _defaultDashPower, ForceMode.Impulse);
    }
    public void AttackRotate()
    {
        if (_playerMove.moveDirection != Vector3.zero)
        {
            transform.forward = _playerMove.moveDirection;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _knockbackDirection = transform.forward + transform.up;

            //연속 엔터 방지
            Weapon.enabled = false;
            _isHit = false;

            Rigidbody rigidbody = other.GetComponent<Rigidbody>();
            if (_isHit == false)
            {
                if (CurrentPossibleComboCount == COMBO_FINISH_COUNT)
                {
                    rigidbody.AddForce(_knockbackDirection * _heavyKnockbackPower, ForceMode.Impulse);
                }
                else
                {
                    rigidbody.AddForce(_knockbackDirection * _lightKnockbackPower, ForceMode.Impulse);
                }

            }
        }
    }
}

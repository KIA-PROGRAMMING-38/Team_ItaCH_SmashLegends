using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerHit : MonoBehaviour
{
    private float _lightKnockbackPower = 0.4f;
    private float _heavyKnockbackPower = 0.7f;
    internal bool invincible;

    private Vector3 _knockbackDirection;

    private PlayerAttack _playerAttack;
    private CharacterStatus _characterStatus;
    private PlayerStatus _playerStatus;
    private Animator _animator;

    void Start()
    {
        _playerAttack = GetComponent<PlayerAttack>();
        _characterStatus = GetComponent<CharacterStatus>();
        _playerStatus = GetComponent<PlayerStatus>();
        _animator = GetComponent<Animator>();

        invincible = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && _playerStatus.CurrentState == PlayerStatus.State.HitUp)
        {
            _animator.SetTrigger(AnimationHash.HitDown);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && (_playerStatus.CurrentState == PlayerStatus.State.ComboAttack
            || _playerStatus.CurrentState == PlayerStatus.State.HeavyAttack))
        {
            Vector3 hitPoint = other.transform.position - transform.position;
            other.transform.rotation = Quaternion.LookRotation(-hitPoint);
            Hit(other);
        }
    }
    private void Hit(Collider other)
    {
        _knockbackDirection = transform.forward + transform.up;
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        Animator animator = other.GetComponent<Animator>();
        PlayerHit playerHit = other.GetComponent<PlayerHit>();
        if (playerHit.invincible == false)
        {
            AttackRangeOff();
            // 공격력 아직 미정
            //_characterStatus.GetDamage();
            if (EndComboAttack())
            {
                animator.Play(AnimationHash.HitUp);
                rigidbody.AddForce(_knockbackDirection * _heavyKnockbackPower, ForceMode.Impulse);
            }
            else if (_playerStatus.CurrentState == PlayerStatus.State.HeavyAttack)
            {
                animator.Play(AnimationHash.HitUp);
                rigidbody.AddForce((_knockbackDirection * 1.3f) * _heavyKnockbackPower, ForceMode.Impulse);
            }
            else if(_playerStatus.CurrentState == PlayerStatus.State.ComboAttack)
            {
                animator.SetTrigger(AnimationHash.Hit);
                rigidbody.AddForce(_knockbackDirection * _lightKnockbackPower, ForceMode.Impulse);
            }
        }
    }

    private bool EndComboAttack() => _playerAttack.CurrentPossibleComboCount == 0;
    public void AttackRangeOn() => _playerAttack.attackRange.enabled = true;
    public void AttackRangeOff() => _playerAttack.attackRange.enabled = false;
}

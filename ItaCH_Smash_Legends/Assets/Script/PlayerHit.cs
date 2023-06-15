using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerHit : MonoBehaviour
{
    private float _lightKnockbackPower = 0.2f;
    private float _heavyKnockbackPower = 0.8f;
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

    private void  OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && (_playerStatus.CurrentState == PlayerStatus.State.ComboAttack
            || _playerStatus.CurrentState == PlayerStatus.State.HeavyAttack
            || _playerStatus.CurrentState == PlayerStatus.State.SkillEndAttack
            || _playerStatus.CurrentState == PlayerStatus.State.SkillAttack))
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
            //AttackRangeOff();
            CharacterStatus opponentCharacter = other.GetComponent<CharacterStatus>();
            if (EndComboAttack())
            {
                GetHit(_lightKnockbackPower, other, AnimationHash.HitUp /*_characterStatus.DefaultAttackDamage*/);
            }
            else if (_playerStatus.CurrentState == PlayerStatus.State.SkillAttack)
            {
                GetHit(_lightKnockbackPower, other, AnimationHash.Hit /*_characterStatus.DefaultAttackDamage*/);
            }
            else if (_playerStatus.CurrentState == PlayerStatus.State.SkillEndAttack)
            {
                GetHit(_heavyKnockbackPower, other, AnimationHash.HitUp /*_characterStatus.HeavyAttackDamage*/);
            }
            else if (_playerStatus.CurrentState == PlayerStatus.State.HeavyAttack)
            {
                GetHit(_heavyKnockbackPower, other, AnimationHash.HitUp /*_characterStatus.HeavyAttackDamage*/);
            }
            else if (_playerStatus.CurrentState == PlayerStatus.State.ComboAttack)
            {
                GetHit(_lightKnockbackPower, other, AnimationHash.Hit /*_characterStatus.DefaultAttackDamage*/);
            }
        }

    }
    private void GetHit(float power, Collider other, int animationHash /*int damage*/)
    {
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        Animator animator = other.GetComponent<Animator>();
        CharacterStatus opponentCharacter = other.GetComponent<CharacterStatus>();

        rigidbody.AddForce(_knockbackDirection * power, ForceMode.Impulse);
        animator.Play(animationHash);
        //opponentCharacter.GetDamage(damage);
    }
    private bool EndComboAttack() => _playerAttack.CurrentPossibleComboCount == 0;
}

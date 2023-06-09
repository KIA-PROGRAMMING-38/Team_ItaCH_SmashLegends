using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    private float _lightKnockbackPower = 0.4f;
    private float _heavyKnockbackPower = 0.7f;
    internal bool invincible;

    private Vector3 _knockbackDirection;

    public GameObject secondPlayer;
    private PlayerAttack _playerAttack;
    private CharacterStatus _characterStatus;
    private PlayerStatus _playerStatus;

    void Start()
    {
        _playerAttack= GetComponent<PlayerAttack>();
        _characterStatus= GetComponent<CharacterStatus>();
        _playerStatus= GetComponent<PlayerStatus>();

        invincible = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && (_playerStatus.CurrentState == PlayerStatus.State.ComboAttack 
            || _playerStatus.CurrentState == PlayerStatus.State.HeavyAttack))
        {
            Vector3 hitPoint = other.transform.position - transform.position;
            other.transform.rotation = Quaternion.LookRotation(-hitPoint);
            Debug.Log(other.name);
            Hit().Forget();
        }
    }
    private async UniTaskVoid Hit()
    {
        _knockbackDirection = transform.forward + transform.up;
        Rigidbody rigidbody = secondPlayer.GetComponent<Rigidbody>();
        Animator animator = secondPlayer.GetComponent<Animator>();
        if (!invincible)
        {
            // 공격력 아직 미정
            //_characterStatus.GetDamage();
            if (EndComboAttack())
            {
                animator.Play(AnimationHash.HitUp);
                rigidbody.AddForce((_knockbackDirection * 1.1f) * _heavyKnockbackPower, ForceMode.Impulse);
            }

            else if(_playerStatus.CurrentState == PlayerStatus.State.HeavyAttack)
            {
                animator.Play(AnimationHash.HitUp);
                rigidbody.AddForce((_knockbackDirection * 1.3f) * _heavyKnockbackPower, ForceMode.Impulse);
            }
            
            else
            {
                animator.SetTrigger(AnimationHash.Hit);
                rigidbody.AddForce(_knockbackDirection * _lightKnockbackPower, ForceMode.Impulse);
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f));
            AttackRangeOff();
            animator.ResetTrigger(AnimationHash.Hit);
        }
    }

    private bool EndComboAttack() => _playerAttack.CurrentPossibleComboCount == 0;
    public void AttackRangeOn() => _playerAttack.attackRange.enabled = true;
    public void AttackRangeOff() => _playerAttack.attackRange.enabled = false;
}

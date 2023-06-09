using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHit : MonoBehaviour
{
    private float _lightKnockbackPower = 0.4f;
    private float _heavyKnockbackPower = 0.7f;
    internal bool invincible;

    private Vector3 _knockbackDirection;

    public GameObject _secondPlayer;
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
        if (other.CompareTag("Player") && _playerStatus.CurrentState == PlayerStatus.State.ComboAttack)
        {
            Vector3 hitPoint = other.transform.position - transform.position;
            other.transform.rotation = Quaternion.LookRotation(-hitPoint);
            Hit().Forget();
        }
    }
    private async UniTaskVoid Hit()
    {
        _knockbackDirection = transform.forward + transform.up;
        Rigidbody rigidbody = _secondPlayer.GetComponent<Rigidbody>();
        Animator animator = _secondPlayer.GetComponent<Animator>();
        if (!invincible)
        {
            // 공격력 아직 미정
            //_characterStatus.GetDamage();
            if (EndComboAttack())
            {
                rigidbody.AddForce(_knockbackDirection * _heavyKnockbackPower, ForceMode.Impulse);
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

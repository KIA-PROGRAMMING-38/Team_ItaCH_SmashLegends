using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;
using UnityEngine.Assertions.Must;

public class AliceHit : PlayerHit
{
    public override void Hit(Collider other)
    {
        lightKnockbackPower = 0.2f;
        heavyKnockbackPower = 0.8f;
        _knockbackDirection = transform.forward + transform.up;

        if (!invincible)
        {
            switch (_playerStatus.CurrentState)
            {
                case PlayerStatus.State.SkillEndAttack:
                    SkillAttackHit(other).Forget();
                    break;
                case PlayerStatus.State.ComboAttack:
                    GetHit(lightKnockbackPower, AnimationHash.Hit, other /*_characterStatus.DefaultAttackDamage*/);
                    break;
                case PlayerStatus.State.FinishComboAttack:
                    GetHit(heavyKnockbackPower, AnimationHash.HitUp, other /*_characterStatus.DefaultAttackDamage*/);
                    break;
            }
        }
    }

    private async UniTask SkillAttackHit(Collider other)
    {
        
        float knockbackPower = 0.8f;
        Vector3 firstKnockbackDirection = transform.up;
        Vector3 secondKnockbackDirection = (transform.position - other.transform.position).normalized;
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        Animator animator = other.GetComponent<Animator>();
        PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();

        playerStatus.CurrentState = PlayerStatus.State.None;
        rigidbody.AddForce(firstKnockbackDirection * knockbackPower, ForceMode.Impulse);
        animator.SetTrigger(AnimationHash.Hit);
        await UniTask.Delay(400);
        rigidbody.AddForce((transform.up + secondKnockbackDirection) * 0.5f, ForceMode.Impulse);
        animator.SetTrigger(AnimationHash.Hit);
        await UniTask.Delay(1000);
        playerStatus.CurrentState = PlayerStatus.State.Idle;
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                    GetHit(heavyKnockbackPower, AnimationHash.Hit, other /*_characterStatus.HeavyAttackDamage*/);
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
}
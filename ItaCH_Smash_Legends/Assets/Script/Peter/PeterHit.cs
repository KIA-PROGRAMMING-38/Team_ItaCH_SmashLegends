using UnityEngine;

public class PeterHit : PlayerHit
{
    public override void Hit(Collider other)
    {
        lightKnockbackPower = 0.2f;
        heavyKnockbackPower = 0.8f;
        _knockbackDirection = transform.forward + transform.up;


        switch (_playerStatus.CurrentState)
        {

            case PlayerStatus.State.SkillAttack:
                GetHit(lightKnockbackPower, AnimationHash.Hit, other, _characterStatus.SkillAttackDamage);
                break;
            case PlayerStatus.State.SkillEndAttack:
                GetHit(heavyKnockbackPower, AnimationHash.HitUp, other, _characterStatus.HeavyAttackDamage);
                break;
            case PlayerStatus.State.HeavyAttack:
                GetHit(heavyKnockbackPower, AnimationHash.HitUp, other, _characterStatus.HeavyAttackDamage);
                break;
            case PlayerStatus.State.ComboAttack:
                GetHit(lightKnockbackPower, AnimationHash.Hit, other, _characterStatus.DefaultAttackDamage);
                break;
            case PlayerStatus.State.FinishComboAttack:
                GetHit(heavyKnockbackPower, AnimationHash.HitUp, other, _characterStatus.DefaultAttackDamage);
                break;
            case PlayerStatus.State.JumpAttack:
                GetHit(heavyKnockbackPower, AnimationHash.HitUp, other, _characterStatus.DefaultAttackDamage);
                break;

        }
    }
    private void GetHit(float power, int animationHash, Collider other, int damage)
    {
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        Animator animator = other.GetComponent<Animator>();
        CharacterStatus opponentCharacter = other.GetComponent<CharacterStatus>();
        if (rigidbody.velocity != Vector3.zero)
        {
            rigidbody.velocity = Vector3.zero;
        }
        rigidbody.AddForce(_knockbackDirection * power, ForceMode.Impulse);
        animator.SetTrigger(animationHash);
        opponentCharacter.GetDamage(damage);
    }
}

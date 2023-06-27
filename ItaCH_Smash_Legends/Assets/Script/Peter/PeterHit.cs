using UnityEngine;

public class PeterHit : PlayerHit
{
    public override void Hit(Collider other)
    {
        _knockbackDirection = transform.forward + transform.up;

        switch (_playerStatus.CurrentState)
        {

            case PlayerStatus.State.SkillAttack:
                GetHit(defaultKnockbackPower, AnimationHash.Hit, other, _characterStatus.SkillAttackDamage);
                break;
            case PlayerStatus.State.SkillEndAttack:
                GetHit(heavyKnockbackPower, AnimationHash.HitUp, other, _characterStatus.HeavyAttackDamage);
                break;
            case PlayerStatus.State.HeavyAttack:
                GetHit(heavyKnockbackPower, AnimationHash.HitUp, other, _characterStatus.HeavyAttackDamage);
                break;
            case PlayerStatus.State.ComboAttack:
                GetHit(defaultKnockbackPower, AnimationHash.Hit, other, _characterStatus.DefaultAttackDamage);
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
        PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
        CharacterStatus opponentCharacter = other.GetComponent<CharacterStatus>();

        if (playerStatus.CurrentState == PlayerStatus.State.Jump && _playerStatus.CurrentState != PlayerStatus.State.JumpAttack)
            return;

        if (rigidbody.velocity != Vector3.zero)
        {
            rigidbody.velocity = Vector3.zero;
        }

        rigidbody.AddForce(_knockbackDirection * power, ForceMode.Impulse);
        animator.SetTrigger(animationHash);
        opponentCharacter.GetDamage(damage);
    }
}

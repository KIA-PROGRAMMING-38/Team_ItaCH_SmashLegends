using Photon.Pun;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AliceHit : PlayerHit
{
    public override void Hit(Collider other)
    {
        Vector3 defaultAttackKnockbackDirection = transform.up;
        Vector3 heavyAttackKnockbackDirection = transform.up + transform.forward;
        lightKnockbackPower = 0.8f;
        heavyKnockbackPower = 0.8f;
        if (!invincible)
        {
            switch (_playerStatus.CurrentState)
            {
                case PlayerStatus.State.SkillEndAttack:
                    SkillAttackHit(other).Forget();
                    break;
                case PlayerStatus.State.ComboAttack:
                    GetHit(defaultAttackKnockbackDirection, lightKnockbackPower, AnimationHash.Hit, other);
                    break;
                case PlayerStatus.State.FinishComboAttack:
                    GetHit(heavyAttackKnockbackDirection, heavyKnockbackPower, AnimationHash.HitUp, other);
                    break;
                case PlayerStatus.State.JumpAttack:
                    GetHit(heavyAttackKnockbackDirection, heavyKnockbackPower, AnimationHash.HitUp, other);
                    break;
            }
        }
    }

    private async UniTask SkillAttackHit(Collider other)
    {

        float knockbackPower = 0.8f;
        float pullingPower = 0.5f;
        Vector3 firstKnockbackDirection = transform.up;
        Vector3 secondKnockbackDirection = (transform.position - other.transform.position).normalized;
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        Animator animator = other.GetComponent<Animator>();
        PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();

        playerStatus.CurrentState = PlayerStatus.State.None;
        rigidbody.AddForce(firstKnockbackDirection * knockbackPower, ForceMode.Impulse);
        animator.SetTrigger(AnimationHash.Hit);
        await UniTask.Delay(400);
        rigidbody.AddForce((transform.up + secondKnockbackDirection) * pullingPower, ForceMode.Impulse);
        animator.SetTrigger(AnimationHash.Hit);
        await UniTask.Delay(1000);
        playerStatus.CurrentState = PlayerStatus.State.Idle;
    }

    private void GetHit(Vector3 transform, float power, int AnimationHash, Collider other)
    {
        lightKnockbackPower = 0.2f;
        heavyKnockbackPower = 0.8f;
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        Animator animator = other.GetComponent<Animator>();

        rigidbody.AddForce(transform * power, ForceMode.Impulse);
        animator.SetTrigger(AnimationHash);
    }
}
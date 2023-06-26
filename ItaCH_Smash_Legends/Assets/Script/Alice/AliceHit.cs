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

        switch (_playerStatus.CurrentState)
        {
            case PlayerStatus.State.SkillAttack:
                SkillAttackHit(other, _characterStatus.SkillAttackDamage).Forget();
                break;
            case PlayerStatus.State.ComboAttack:
                GetHit(defaultAttackKnockbackDirection, lightKnockbackPower, AnimationHash.Hit, other, _characterStatus.DefaultAttackDamage);
                break;
            case PlayerStatus.State.FinishComboAttack:
                GetHit(heavyAttackKnockbackDirection, heavyKnockbackPower, AnimationHash.HitUp, other, _characterStatus.HeavyAttackDamage);
                break;
            case PlayerStatus.State.JumpAttack:
                GetHit(heavyAttackKnockbackDirection, heavyKnockbackPower, AnimationHash.HitUp, other, _characterStatus.JumpAttackDamage);
                break;
        }

    }

    private async UniTask SkillAttackHit(Collider other, int damage)
    {

        float knockbackPower = 0.8f;
        float pullingPower = 0.5f;
        Vector3 firstKnockbackDirection = transform.up;
        Vector3 secondKnockbackDirection = (transform.position - other.transform.position).normalized;
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        Animator animator = other.GetComponent<Animator>();
        PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
        CharacterStatus opponentCharacter = other.GetComponent<CharacterStatus>();

        playerStatus.CurrentState = PlayerStatus.State.None;
        rigidbody.AddForce(firstKnockbackDirection * knockbackPower, ForceMode.Impulse);
        animator.SetTrigger(AnimationHash.Hit);
        // damage의 경우 InGame에서 확인을 해야해서 주석처리해뒀습니다.
        //opponentCharacter.GetDamage(damage);
        await UniTask.Delay(400);
        rigidbody.AddForce((transform.up + secondKnockbackDirection) * pullingPower, ForceMode.Impulse);
        animator.SetTrigger(AnimationHash.Hit);
        //opponentCharacter.GetDamage(damage);
        await UniTask.Delay(1000);
        playerStatus.CurrentState = PlayerStatus.State.Idle;
    }

    private void GetHit(Vector3 transform, float power, int AnimationHash, Collider other, int damage)
    {
        lightKnockbackPower = 0.2f;
        heavyKnockbackPower = 0.8f;

        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        Animator animator = other.GetComponent<Animator>();
        PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
        CharacterStatus opponentCharacter = other.GetComponent<CharacterStatus>();

        if (playerStatus.CurrentState == PlayerStatus.State.Jump && _playerStatus.CurrentState != PlayerStatus.State.JumpAttack)
            return;

        rigidbody.AddForce(transform * power, ForceMode.Impulse);
        animator.SetTrigger(AnimationHash);
        opponentCharacter.GetDamage(damage);
    }
}
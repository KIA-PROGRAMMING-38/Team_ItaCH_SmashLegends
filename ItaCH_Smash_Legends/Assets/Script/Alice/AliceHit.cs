using Cysharp.Threading.Tasks;
using UnityEngine;

public class AliceHit : PlayerHit
{
    public override void Hit(Collider other)
    {
        Vector3 defaultAttackKnockbackDirection = transform.up;
        Vector3 heavyAttackKnockbackDirection = transform.up + transform.forward;

        switch (_playerStatus.CurrentState)
        {
            case PlayerStatus.State.SkillAttack:
                SkillAttackHit(other, _characterStatus.SkillAttackDamage).Forget();
                break;
            case PlayerStatus.State.ComboAttack:
                GetHit(defaultAttackKnockbackDirection, defaultKnockbackPower, AnimationHash.Hit, other,_characterStatus.DefaultAttackDamage);
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
        int afterSmashDelay = 400;
        int unControllableMovement = 1000;
        Vector3 knockbackDirection = (transform.position - other.transform.position).normalized;
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        Animator animator = other.GetComponent<Animator>();
        PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
        CharacterStatus opponentCharacter = other.GetComponent<CharacterStatus>();

        playerStatus.CurrentState = PlayerStatus.State.None;
        rigidbody.AddForce(Vector3.up * knockbackPower, ForceMode.Impulse);
        animator.SetTrigger(AnimationHash.Hit);
        // damage의 경우 InGame에서 확인을 해야해서 주석처리해뒀습니다.
        //opponentCharacter.GetDamage(damage);
        await UniTask.Delay(afterSmashDelay);
        rigidbody.AddForce((Vector3.up + knockbackDirection) * pullingPower, ForceMode.Impulse);
        animator.SetTrigger(AnimationHash.Hit);
        //opponentCharacter.GetDamage(damage);
        await UniTask.Delay(unControllableMovement);
        playerStatus.CurrentState = PlayerStatus.State.Idle;
    }

    public void GetHit(Vector3 transform, float power, int AnimationHash, Collider other, int damage)
    {
        defaultKnockbackPower = 0.2f;
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
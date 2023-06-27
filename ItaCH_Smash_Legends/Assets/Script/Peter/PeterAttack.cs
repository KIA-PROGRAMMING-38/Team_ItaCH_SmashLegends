using UnityEngine;

public class PeterAttack : PlayerAttack
{
    private float _skillAttackMoveSpeed = 7f;
    [SerializeField] private SphereCollider _skillAttackHitZone;
    [SerializeField] private SphereCollider _attackHitZone;
    [SerializeField] private SphereCollider _heavyAttackHitZone;
    [SerializeField] private BoxCollider _jumpAttackHitZone;

    public override void AttackOnDash()
    {
        defaultDashPower = 0.8f;
        rigidbodyAttack.AddForce(transform.forward * defaultDashPower, ForceMode.Impulse);
    }

    private void Update()
    {
        if (playerStatus.CurrentState == PlayerStatus.State.SkillAttack)
        {
            rigidbodyAttack.velocity = transform.forward * _skillAttackMoveSpeed;
            if (playerStatus.CurrentState == PlayerStatus.State.SkillEndAttack)
            {
                rigidbodyAttack.velocity = Vector3.zero;
            }
        }
    }
    public override void DefaultAttack()
    {
        if (IsPossibleFirstAttack())
        {
            playerStatus.CurrentState = PlayerStatus.State.ComboAttack;
            animator.Play(AnimationHash.FirstAttack);
        }

        if (isFirstAttack && CurrentPossibleComboCount == COMBO_SECOND_COUNT)
        {
            isSecondAttack = true;
        }
        if (isSecondAttack && CurrentPossibleComboCount == COMBO_FINISH_COUNT)
        {
            isFinishAttack = true;
        }
    }

    public override void SkillAttack()
    {
        if (playerStatus.CurrentState == PlayerStatus.State.Run ||
            playerStatus.CurrentState == PlayerStatus.State.Idle)
        {
            animator.Play(AnimationHash.SkillAttack);
        }
    }

    private void EnableAttackHitZone() => _attackHitZone.enabled = true;
    private void DisableAttackHitZone() => _attackHitZone.enabled = false;
    private void EnableJumpAttackHitZone() => _jumpAttackHitZone.enabled = true;
    private void DisableJumpAttackHitZone() => _jumpAttackHitZone.enabled = false;
    private void EnableHeavyAttackHitZone() => _heavyAttackHitZone.enabled = true;
    private void DisableHeavyAttackHitZone() => _heavyAttackHitZone.enabled = false;
    private void EnableSkillAttackHitZone() => _skillAttackHitZone.enabled = true;
    private void DisableSkillAttackHitZone() => _skillAttackHitZone.enabled = false;
    private void ChangeSkillEndAttackStatus() => playerStatus.CurrentState = PlayerStatus.State.SkillEndAttack;
}

using UnityEngine;

public class AliceAttack : PlayerAttack
{
    [SerializeField] private GameObject _aliceBomb;
    [SerializeField] private BoxCollider _attackHitZone;
    [SerializeField] private SphereCollider _finishAttackHitZone;
    [SerializeField] private SphereCollider _skillAttackHitZone;
    [SerializeField] private BoxCollider _jumpAttackHitZone;

    public override void DashOnAnimationEvent()
    {
        attackRigidbody.AddForce(transform.forward * legendController.Stat.DashPower, ForceMode.Impulse);
    }
    private void HeavyAttackBomb() => _aliceBomb.gameObject.SetActive(true);
    private void EnableAttackHitZone() => _attackHitZone.enabled = true;
    private void DisableAttackHitZone() => _attackHitZone.enabled = false;
    private void EnableFinishAttackHitZone() => _finishAttackHitZone.enabled = true;
    private void DisableFinishAttackHitZone() => _finishAttackHitZone.enabled = false;
    private void EnableJumpAttackHitZone() => _jumpAttackHitZone.enabled = true;
    private void DisableJumpAttackHitZone() => _jumpAttackHitZone.enabled = false;
    private void EnableSkillAttackHitZone() => _skillAttackHitZone.enabled = true;
    private void DisableSkillAttackHitZone() => _skillAttackHitZone.enabled = false;
}

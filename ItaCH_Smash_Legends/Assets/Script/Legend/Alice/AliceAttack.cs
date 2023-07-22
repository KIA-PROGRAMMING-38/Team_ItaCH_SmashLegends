using UnityEngine;

public class AliceAttack : PlayerAttack
{
    [SerializeField] private GameObject _aliceBomb;
    [SerializeField] private Collider _attackHitZone;
    [SerializeField] private Collider _finishAttackHitZone;
    [SerializeField] private Collider _skillAttackHitZone;
    [SerializeField] private Collider _jumpAttackHitZone;

    private Vector3 _skillAttackDirection;
    
    private void DashAtSkillAttackOnAnimationEvent()
    {
        _skillAttackDirection = transform.up + transform.forward;
        attackRigidbody.AddForce(_skillAttackDirection * 1f, ForceMode.Impulse);
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

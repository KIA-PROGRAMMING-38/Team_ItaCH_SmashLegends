using UnityEngine;

public class AliceAttack : PlayerAttack
{
    [SerializeField] private GameObject _aliceBomb;
    [SerializeField] private GameObject _attackHitZone;
    [SerializeField] private GameObject _finishAttackHitZone;
    [SerializeField] private GameObject _skillAttackHitZone;
    [SerializeField] private GameObject _jumpAttackHitZone;

    private Vector3 _skillAttackDirection;

    private void DashAtSkillAttackOnAnimationEvent()
    {
        _skillAttackDirection = transform.up + transform.forward;
        attackRigidbody.AddForce(_skillAttackDirection * 1f, ForceMode.Impulse);
    }
    private void HeavyAttackBomb() => _aliceBomb.SetActive(true);
    private void EnableAttackHitZone() => _attackHitZone.SetActive(true);
    private void DisableAttackHitZone() => _attackHitZone.SetActive(false);
    private void EnableFinishAttackHitZone() => _finishAttackHitZone.SetActive(true);
    private void DisableFinishAttackHitZone() => _finishAttackHitZone.SetActive(false);
    private void EnableJumpAttackHitZone() => _jumpAttackHitZone.SetActive(true);
    private void DisableJumpAttackHitZone() => _jumpAttackHitZone.SetActive(false);
    private void EnableSkillAttackHitZone() => _skillAttackHitZone.SetActive(true);
    private void DisableSkillAttackHitZone() => _skillAttackHitZone.SetActive(false);
}

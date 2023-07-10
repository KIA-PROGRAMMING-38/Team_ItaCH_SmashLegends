using UnityEngine;

public class AliceAttack : PlayerAttack
{
    [SerializeField] private GameObject _aliceBomb;
    [SerializeField] private BoxCollider _attackHitZone;
    [SerializeField] private SphereCollider _finishAttackHitZone;
    [SerializeField] private SphereCollider _skillAttackHitZone;
    [SerializeField] private BoxCollider _jumpAttackHitZone;
    private Rigidbody _rigidbody;
    
    private void Start()
    {
        CurrentPossibleComboCount = MAX_POSSIBLE_ATTACK_COUNT;
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override void DashOnAnimationEvent()
    {
        defaultDashPower = 0.7f;
        if (playerStatus.CurrentState == PlayerStatus.State.ComboAttack)
        {
            rigidbodyAttack.AddForce(transform.forward * defaultDashPower, ForceMode.Impulse);
        }
    }

    public override void DefaultAttack()
    {        
        if (IsPossibleFirstAttack())
        {
            isFinishAttack = false;
            playerStatus.CurrentState = PlayerStatus.State.ComboAttack;
            DashOnAnimationEvent();
            animator.Play(AnimationHash.FirstAttack);
            --CurrentPossibleComboCount;            
        }
        if (isFirstAttack && CurrentPossibleComboCount == COMBO_FINISH_COUNT)
        {
            isFinishAttack = true;
        }
    }

    public override void HeavyAttack()
    {
        if (playerStatus.CurrentState == PlayerStatus.State.Run ||
           playerStatus.CurrentState == PlayerStatus.State.Idle)
        {
            animator.Play(AnimationHash.HeavyAttack);
            playerStatus.CurrentState = PlayerStatus.State.HeavyAttack;
        }
    }

    public override void SkillAttack()
    {
        if (playerStatus.CurrentState == PlayerStatus.State.Run
            || playerStatus.CurrentState == PlayerStatus.State.Idle)

        {
            Vector3 direction = (transform.forward + transform.up).normalized;
            _rigidbody.AddForce(direction * 1f, ForceMode.Impulse);
            animator.Play(AnimationHash.SkillAttack);
        }
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

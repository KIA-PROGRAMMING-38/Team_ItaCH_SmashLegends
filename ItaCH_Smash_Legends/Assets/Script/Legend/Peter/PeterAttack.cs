using UnityEngine;
using Cysharp.Threading.Tasks;

public class PeterAttack : PlayerAttack
{  
    private float _skillAttackMoveSpeed = 7f;
    [SerializeField] private SphereCollider _skillAttackHitZone;
    [SerializeField] private SphereCollider _attackHitZone;
    [SerializeField] private SphereCollider _heavyAttackHitZone;
    [SerializeField] private BoxCollider _jumpAttackHitZone;

    public override void DashOnAnimationEvent()
    {
        attackRigidbody.AddForce(transform.forward * legendController.Stat.DashPower, ForceMode.Impulse);
    }


    private async UniTaskVoid MoveSkillAttack()
    {
        //if (playerStatus.CurrentState == PlayerStatus.State.SkillAttack)
        //{
        //    rigidbodyAttack.velocity = transform.forward * _skillAttackMoveSpeed;
        //    await UniTask.Delay(3000);
        //    if (playerStatus.CurrentState == PlayerStatus.State.SkillEndAttack)
        //    {
        //        rigidbodyAttack.velocity = Vector3.zero;
        //    }
        //}
    }
    private void EnableAttackHitZone() => _attackHitZone.enabled = true;
    private void DisableAttackHitZone() => _attackHitZone.enabled = false;
    private void EnableJumpAttackHitZone() => _jumpAttackHitZone.enabled = true;
    private void DisableJumpAttackHitZone() => _jumpAttackHitZone.enabled = false;
    private void EnableHeavyAttackHitZone() => _heavyAttackHitZone.enabled = true;
    private void DisableHeavyAttackHitZone() => _heavyAttackHitZone.enabled = false;
    private void EnableSkillAttackHitZone() => _skillAttackHitZone.enabled = true;
    private void DisableSkillAttackHitZone() => _skillAttackHitZone.enabled = false;
    //private void ChangeSkillEndAttackStatus() => playerStatus.CurrentState = PlayerStatus.State.SkillEndAttack;
}

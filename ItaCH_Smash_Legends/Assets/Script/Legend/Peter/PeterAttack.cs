using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class PeterAttack : PlayerAttack
{
    // LegnedController 완료시 리펙토링
    private float _skillAttackMoveSpeed = 7f;

    [SerializeField] private Collider _skillAttackHitZone;
    [SerializeField] private Collider _attackHitZone;
    [SerializeField] private Collider _heavyAttackHitZone;
    [SerializeField] private Collider _jumpAttackHitZone;

    private CancellationTokenSource _cancelSource;

    public override void DashOnAnimationEvent()
    {
        //TODO : 캐릭터 스탯 변경 후 적용
        //attackRigidbody.AddForce(transform.forward * characterStatus.Stat.DashPower, ForceMode.Impulse);
        attackRigidbody.velocity = Vector3.zero;
        attackRigidbody.AddForce(transform.forward * 0.8f, ForceMode.Impulse);

    }
    private void StopSkillAttackOnAnimationEvent()
    {
        _cancelSource.Cancel();
    }
    private void StartSkillAttackOnAnimationEvent()
    {
        MoveAtSkillAttack().Forget();
    }
    private async UniTaskVoid MoveAtSkillAttack()
    {
        _cancelSource = new CancellationTokenSource();

        while (true)
        {
            attackRigidbody.velocity = transform.forward * _skillAttackMoveSpeed;
            await UniTask.Delay(1, cancellationToken: _cancelSource.Token);

            attackRigidbody.velocity = Vector3.zero;
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
}

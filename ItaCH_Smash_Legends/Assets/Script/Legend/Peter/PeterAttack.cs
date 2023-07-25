using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class PeterAttack : PlayerAttack
{
    // LegnedController 완료시 리펙토링
    private float _skillAttackMoveSpeed = 7f;

    [SerializeField] private GameObject _skillAttackHitZone;
    [SerializeField] private GameObject _attackHitZone;
    [SerializeField] private GameObject _heavyAttackHitZone;
    [SerializeField] private GameObject _jumpAttackHitZone;

    private CancellationTokenSource _cancelSource;

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
    private void EnableAttackHitZone() => _attackHitZone.SetActive(true);
    private void DisableAttackHitZone() => _attackHitZone.SetActive(false);
    private void EnableJumpAttackHitZone() => _jumpAttackHitZone.SetActive(true);
    private void DisableJumpAttackHitZone() => _jumpAttackHitZone.SetActive(false);
    private void EnableHeavyAttackHitZone() => _heavyAttackHitZone.SetActive(true);
    private void DisableHeavyAttackHitZone() => _heavyAttackHitZone.SetActive(false);
    private void EnableSkillAttackHitZone() => _skillAttackHitZone.SetActive(true);
    private void DisableSkillAttackHitZone() => _skillAttackHitZone.SetActive(false);
}

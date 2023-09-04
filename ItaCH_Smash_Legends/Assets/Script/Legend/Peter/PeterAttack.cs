using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class PeterAttack : PlayerAttack
{
    enum DefaultAttackType
    {
        First,
        SecondOne,
        SecondTwo,
        Finish
    }

    private float _skillAttackMoveSpeed = 7f;

    [SerializeField] private GameObject _skillAttackHitZone;
    [SerializeField] private GameObject _attackHitZone;
    [SerializeField] private GameObject _heavyAttackHitZone;
    [SerializeField] private GameObject _jumpAttackHitZone;
    [SerializeField] private GameObject _finishDefaultAttackHitZone;

    private CancellationTokenSource _cancelSource;
    private float _correctionPower = 0.8f;

    private string[] _attacks;

    private void Start()
    {
        dashPower = legendController.Stat.DashPower * _correctionPower;
        _attacks = new string[] { StringLiteral.SFX_DEFAULTATTACK_ZERO, StringLiteral.SFX_DEFAULTATTACK_ONE,
            StringLiteral.SFX_DEFAULTATTACK_TWO, StringLiteral.SFX_DEFAULTATTACK_THREE, };
    }
    private void StopSkillAttackOnAnimationEvent()
    {
        _cancelSource.Cancel();
    }
    private void StartSkillAttackOnAnimationEvent()
    {
        MoveAtSkillAttack().Forget();
    }
    private void PlaySFXAttackSound(DefaultAttackType attackType)
    {
        Managers.SoundManager.Play(SoundType.SFX, _attacks[(int)attackType], legend: LegendType.Peter);
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
    private void EnableFinishAttackHitZone() => _finishDefaultAttackHitZone.SetActive(true);
    private void DisableFinishAttackHitZone() => _finishDefaultAttackHitZone.SetActive(false);
    private void EnableJumpAttackHitZone() => _jumpAttackHitZone.SetActive(true);
    private void DisableJumpAttackHitZone() => _jumpAttackHitZone.SetActive(false);
    private void EnableHeavyAttackHitZone() => _heavyAttackHitZone.SetActive(true);
    private void DisableHeavyAttackHitZone() => _heavyAttackHitZone.SetActive(false);
    private void EnableSkillAttackHitZone() => _skillAttackHitZone.SetActive(true);
    private void DisableSkillAttackHitZone() => _skillAttackHitZone.SetActive(false);
}

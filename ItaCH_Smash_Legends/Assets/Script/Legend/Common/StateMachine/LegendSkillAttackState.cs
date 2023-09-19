using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class LegendSkillAttackState : LegendBaseState
{
    PlayerAttack _playerAttack;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        _playerAttack = animator.GetComponent<PlayerAttack>();
        _playerAttack.CanSkillAttack = false;

        Managers.SoundManager.Play(SoundType.Voice, legend: legendController.LegendType, voice: VoiceType.SkillAttack);
        Managers.SoundManager.Play(SoundType.SFX, StringLiteral.SFX_SKILLATTACK, legendController.LegendType);

        StartCooltime().Forget();
    }

    public async UniTaskVoid StartCooltime()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(legendController.Stat.HeavyCooltime));
        _playerAttack.CanSkillAttack = true;
    }
}

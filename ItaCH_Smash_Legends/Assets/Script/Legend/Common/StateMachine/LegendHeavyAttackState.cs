using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class LegendHeavyAttackState : LegendBaseState
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        legendController.CanHeavyAttack = false;
        animator.SetBool(AnimationHash.Run, false);
        Managers.SoundManager.Play(SoundType.Voice, legend: legendController.LegendType, voice: VoiceType.HeavyAttack);
        Managers.SoundManager.Play(SoundType.SFX, StringLiteral.SFX_HEAVYATTACK, legendController.LegendType);

        StartCooltime().Forget();
    }

    public async UniTaskVoid StartCooltime()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(legendController.Stat.HeavyCooltime));
        legendController.CanHeavyAttack = true;
    }
}

using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class LegendSkillAttackState : LegendBaseState
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        legendController.CanSkillAttack = false;

        Managers.SoundManager.Play(SoundType.Voice, legend: legendController.LegendType, voice: VoiceType.SkillAttack);
        Managers.SoundManager.Play(SoundType.SFX, StringLiteral.SFX_SKILLATTACK, legendController.LegendType);

        StartCooltime().Forget();
    }

    public async UniTaskVoid StartCooltime()
    {
        float skillCooltime = legendController.Stat.HeavyCooltime * 3f;
        await UniTask.Delay(TimeSpan.FromSeconds(skillCooltime));
        legendController.CanSkillAttack = true;
    }
}

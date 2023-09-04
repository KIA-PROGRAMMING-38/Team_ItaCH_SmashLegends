using UnityEngine;

public class LegendHeavyAttackState : LegendBaseState
{
    private PlayerAttack _playerAttack;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        animator.SetBool(AnimationHash.Run, false);
        Managers.SoundManager.Play(SoundType.Voice, legend: legendController.LegendType, voice: VoiceType.HeavyAttack);
        Managers.SoundManager.Play(SoundType.SFX, StringLiteral.SFX_HEAVYATTACK, legendController.LegendType);
    }
}

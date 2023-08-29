using UnityEngine;

public class LegendSkillAttackState : LegendBaseState
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        Managers.SoundManager.Play(SoundType.Voice, legend: legendController.LegendType, voice: VoiceType.SkillAttack);
    }
}

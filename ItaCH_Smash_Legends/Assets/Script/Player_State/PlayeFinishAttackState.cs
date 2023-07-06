using UnityEngine;

public class LegendFinishAttackSt: LegendBaseState
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        legendController.SetComboImpossibleOnAnimationEvent();
        legendController.PlayNextClipOnAnimationEvent();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            legendController.PlayComboAttack(ComboAttackType.First);

    }
}

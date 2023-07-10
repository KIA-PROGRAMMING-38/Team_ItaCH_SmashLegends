using UnityEngine;

public class LegendIdleState : LegendBaseState
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animator.SetBool(AnimationHash.Run, false);
        legendAnimationController.ResetAllAnimatorTriggers(animator);

        legendAnimationController.ResetComboAttackAnimationClip();
        legendController.SetComboImpossibleOnAnimationEvent();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        legendController.MoveAndRotate();
    }
}

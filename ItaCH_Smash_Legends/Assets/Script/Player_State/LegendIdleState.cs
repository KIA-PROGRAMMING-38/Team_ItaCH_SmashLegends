using UnityEngine;

public class LegendIdleState : LegendBaseState
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        animator.ResetTrigger(AnimationHash.RollUpBack);
        animator.ResetTrigger(AnimationHash.RollUpFront);

        legendController.PossibleComboCount = 0;
        legendController.AnimationClipIndex = 0;
        legendController.SetComboImpossibleOnAnimationEvent();

        if (legendController.MoveDirection != Vector3.zero)
        {
            animator.SetBool(AnimationHash.Run, true);
        }
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        legendController.SetNextAnimation();
    }
}

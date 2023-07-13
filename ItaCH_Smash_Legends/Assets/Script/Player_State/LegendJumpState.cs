using UnityEngine;

public class LegendJumpState : LegendBaseState
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        legendController.OnJumping();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        legendController.MoveAndRotate();
        if(legendController.IsTriggered(ActionType.DefaultAttack))
        {
            animator.Play(AnimationHash.FirstJumpAttack);
        }
        if (legendController.IsFalling())
        {
            legendAnimationController.SetBool(AnimationHash.JumpDown, true);
        }
    }
}

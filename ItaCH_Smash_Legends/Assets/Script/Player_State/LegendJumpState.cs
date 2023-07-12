using System.Linq.Expressions;
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
        legendAnimationController.PlayJumpAttackAnimation();
        if (legendController.IsFalling())
        {
            legendAnimationController.Animator.SetBool(AnimationHash.JumpDown, true);
        }
    }
}

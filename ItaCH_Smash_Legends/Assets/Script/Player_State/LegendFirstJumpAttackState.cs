using UnityEngine;

public class LegendFirstJumpAttackState : LegendBaseState
{
    // LegnedController 완료시 리펙토링

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animator.SetBool(AnimationHash.JumpDown, true); 
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        legendController.JumpMoveAndRotate();
        legendController.PlayComboAttack(ComboAttackType.SecondJump);
    }

}

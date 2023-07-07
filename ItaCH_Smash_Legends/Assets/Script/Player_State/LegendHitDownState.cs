using UnityEngine;

public class LegendHitDownState : LegendBaseState
{
    // LegnedController �Ϸ�� �����丵

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        animator.ResetTrigger(AnimationHash.HitDown);
        animator.ResetTrigger(AnimationHash.JumpAttack);
    }
}

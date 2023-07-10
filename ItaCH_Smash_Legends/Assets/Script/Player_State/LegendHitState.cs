using UnityEngine;

public class LegendHitState : LegendBaseState
{
    // LegnedController 완료시 리펙토링

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        animator.ResetTrigger(AnimationHash.Hit);
    }
}

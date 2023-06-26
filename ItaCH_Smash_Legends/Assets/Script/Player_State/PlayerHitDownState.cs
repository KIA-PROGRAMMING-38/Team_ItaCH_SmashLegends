using UnityEngine;

public class PlayerHitDownState : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(AnimationHash.HitDown);
        animator.ResetTrigger(AnimationHash.JumpAttack);
    }
}

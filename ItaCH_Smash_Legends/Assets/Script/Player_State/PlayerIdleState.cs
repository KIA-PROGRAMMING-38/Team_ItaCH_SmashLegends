using UnityEngine;

public class PlayerIdleState : StateMachineBehaviour
{
    private LegendController _legendController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _legendController = animator.GetComponent<LegendController>();

        animator.ResetTrigger(AnimationHash.RollUpBack);
        animator.ResetTrigger(AnimationHash.RollUpFront);

        _legendController.PossibleComboCount = 0;
        _legendController.AnimationClipIndex = 0;
        _legendController.SetComboImpossible();

        if (_legendController.MoveDirection != Vector3.zero)
        {
            animator.SetBool(AnimationHash.Run, true);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _legendController.SetNextAnimation();
    }
}

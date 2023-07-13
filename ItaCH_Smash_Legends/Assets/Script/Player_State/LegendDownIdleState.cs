using JetBrains.Annotations;
using UnityEngine;

public class LegendDownIdleState : LegendBaseState
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (legendController.IsTriggered(ActionType.Move))
        {
            PlayRollingAnimation();
        }
    }

    private void PlayRollingAnimation()
    {
        legendController.SetRollingDirection();
    }
}

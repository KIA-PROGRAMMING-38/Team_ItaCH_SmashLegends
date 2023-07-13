using UnityEngine;

public class LegendDownIdleState : LegendBaseState
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator,stateInfo, layerIndex);
        legendController.StartRollingAsync().Forget();
    }
}

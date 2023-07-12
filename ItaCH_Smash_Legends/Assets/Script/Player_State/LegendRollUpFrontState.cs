using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendRollUpFrontState : LegendBaseState
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        legendController.RollingDash();
    }
}

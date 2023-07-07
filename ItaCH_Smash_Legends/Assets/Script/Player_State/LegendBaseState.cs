using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendBaseState : StateMachineBehaviour
{
    protected LegendController legendController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        legendController = animator.GetComponent<LegendController>();
    }
}

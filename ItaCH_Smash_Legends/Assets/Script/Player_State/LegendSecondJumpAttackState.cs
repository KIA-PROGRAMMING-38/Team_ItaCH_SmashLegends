using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendSecondJumpAttackState : LegendBaseState
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        legendController.PlayComboAttack(ComboAttackType.FirstJump);
    }
}

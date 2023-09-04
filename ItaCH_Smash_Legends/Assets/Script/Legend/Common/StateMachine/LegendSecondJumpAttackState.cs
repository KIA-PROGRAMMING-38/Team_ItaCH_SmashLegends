using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendSecondJumpAttackState : LegendBaseState
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        Managers.SoundManager.Play(SoundType.SFX, StringLiteral.SFX_JUMPATTACK, legendController.LegendType);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        legendController.PlayComboAttack(ComboAttackType.FirstJump);
    }
}

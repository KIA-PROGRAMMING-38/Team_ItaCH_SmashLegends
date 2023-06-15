using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliceAttack : PlayerAttack
{
    private void Start()
    {
        CurrentPossibleComboCount = MAX_POSSIBLE_ATTACK_COUNT;
    }
    
    public override void DefaultAttack()
    {
        if (IsPossibleFirstAttack())
        {
            animator.Play(AnimationHash.FirstAttack);
            --CurrentPossibleComboCount;
        }
        if (isFirstAttack && CurrentPossibleComboCount == COMBO_FINISH_COUNT)
        {
            isSecondAttack = true;
        }
    }

    public override void JumpAttack()
    {
        if (playerStatus.IsJump == false)
        {
            animator.SetTrigger(AnimationHash.JumpAttack);
        }
    }
}

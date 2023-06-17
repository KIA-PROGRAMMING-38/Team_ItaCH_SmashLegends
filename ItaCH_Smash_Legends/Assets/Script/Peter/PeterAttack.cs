using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeterAttack : PlayerAttack
{
    public override void AttackOnDash()
    {
        rigidbodyAttack.AddForce(transform.forward * defaultDashPower, ForceMode.Impulse);
    }

    public override void DefaultAttack()
    {
        if (IsPossibleFirstAttack())
        {
            playerStatus.CurrentState = PlayerStatus.State.ComboAttack;
            animator.Play(AnimationHash.FirstAttack);
        }

        if (isFirstAttack && CurrentPossibleComboCount == COMBO_SECOND_COUNT)
        {
            isSecondAttack = true;
        }
        if (isSecondAttack && CurrentPossibleComboCount == COMBO_FINISH_COUNT)
        {
            isFinishAttack = true;
            playerStatus.CurrentState = PlayerStatus.State.FinishComboAttack;
        }
    }
}

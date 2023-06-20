using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeterAttack : PlayerAttack
{
    private float _skillAttackMoveSpeed = 7f;
    public override void AttackOnDash()
    {
        rigidbodyAttack.AddForce(transform.forward * defaultDashPower, ForceMode.Impulse);
    }

    private void Update()
    {
        if(playerStatus.CurrentState == PlayerStatus.State.SkillAttack)
        {
            rigidbodyAttack.velocity = transform.forward * _skillAttackMoveSpeed;
            if(playerStatus.CurrentState == PlayerStatus.State.SkillEndAttack)
            {
                rigidbodyAttack.velocity = Vector3.zero;
            }
        }
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

    public override void SkillAttack()
    {
        if (playerStatus.CurrentState == PlayerStatus.State.Run ||
            playerStatus.CurrentState == PlayerStatus.State.Idle)
        {
            animator.Play(AnimationHash.SkillAttack);
            playerStatus.CurrentState = PlayerStatus.State.SkillAttack;
        }
    }
}

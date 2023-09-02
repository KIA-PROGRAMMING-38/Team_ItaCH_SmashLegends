using UnityEngine;

public class LegendFirstJumpAttackState : LegendBaseState
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animator.SetBool(AnimationHash.JumpDown, true);
        Managers.SoundManager.Play(SoundType.SFX, StringLiteral.SFX_JUMPATTACK, legendController.LegendType);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        legendController.MoveAndRotate();
        legendController.PlayComboAttack(ComboAttackType.SecondJump);
    }
}

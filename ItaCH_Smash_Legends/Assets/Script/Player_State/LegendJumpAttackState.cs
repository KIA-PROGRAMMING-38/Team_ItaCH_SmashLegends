using UnityEngine;

public class LegendJumpAttackState : LegendBaseState
{
    // LegnedController �Ϸ�� �����丵

    private PlayerJump _playerJump;
    private PlayerStatus _playerStatus;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        _playerJump = animator.GetComponent<PlayerJump>();
        _playerStatus = animator.GetComponent<PlayerStatus>();
        animator.SetBool(AnimationHash.JumpDown, true);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerJump.JumpMoveAndRotate();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(AnimationHash.JumpAttack);
    }
}

using UnityEngine;

public class LegendJumpDownState : LegendBaseState
{
    // LegnedController 완료시 리펙토링

    private PlayerJump _playerJump;
    private PlayerStatus _playerStatus;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        _playerJump = animator.GetComponent<PlayerJump>();
        _playerStatus = animator.GetComponent<PlayerStatus>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.3f)
            _playerJump.JumpMoveAndRotate();

        if (_playerStatus.CurrentState == PlayerStatus.State.JumpAttack)
        {
            animator.ResetTrigger(AnimationHash.JumpAttack);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(AnimationHash.JumpDown, false);
    }
}

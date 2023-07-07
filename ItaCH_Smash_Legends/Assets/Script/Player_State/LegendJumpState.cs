using UnityEngine;

public class LegendJumpState : LegendBaseState
{
    // LegnedController 완료시 리펙토링

    private PlayerJump _playerJump;
    private Rigidbody _rigidbody;
    private PlayerStatus _playerStatus;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        _playerJump = animator.GetComponent<PlayerJump>();
        _rigidbody = animator.GetComponent<Rigidbody>();
        _playerStatus = animator.GetComponent<PlayerStatus>();

        _playerStatus.IsJump = false;
        _playerStatus.IsHang = false;

        _playerStatus.CurrentState = PlayerStatus.State.Jump;
        _playerJump.JumpInput();
        animator.SetBool(AnimationHash.Run, false);

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        legendController.JumpMoveAndRotate();
        if (_rigidbody.velocity.y <= -1)
        {
            animator.SetBool(AnimationHash.JumpDown, true);
        }
    }
}

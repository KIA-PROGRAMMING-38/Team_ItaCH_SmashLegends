using UnityEngine;

public class LegendJumpState : LegendBaseState
{
    private Rigidbody _rigidbody;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _rigidbody = animator.GetComponent<Rigidbody>();
        legendController.isJump = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        legendController.MoveAndRotate();

        if (_rigidbody.velocity.y <= -1)
        {
            legendAnimationController.TrueAnimation(AnimationHash.JumpDown);
        }
    }
}

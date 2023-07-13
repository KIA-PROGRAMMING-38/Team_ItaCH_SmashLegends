using UnityEngine;

public class LegendJumpState : LegendBaseState
{
    private Rigidbody _rigidbody;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _rigidbody = animator.GetComponent<Rigidbody>();

        Jump(animator);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        legendController.MoveAndRotate();
        if (legendController.IsTriggered(ActionType.DefaultAttack))
        {
            animator.Play(AnimationHash.FirstJumpAttack);
        }
        if (legendController.IsFalling())
        {
            legendAnimationController.SetBool(AnimationHash.JumpDown, true);
        }
    }

    private void Jump(Animator animator)
    {
        _rigidbody.AddForce(animator.transform.up * LegendController.MAX_JUMP_POWER, ForceMode.Impulse);
    }
}

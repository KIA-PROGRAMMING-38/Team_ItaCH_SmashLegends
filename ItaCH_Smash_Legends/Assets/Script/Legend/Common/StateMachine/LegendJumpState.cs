using UnityEngine;

public class LegendJumpState : LegendBaseState
{
    private Rigidbody _rigidbody;
    private Vector3 _moveDirection = Vector3.forward * 0.7f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        base.OnStateEnter(animator, stateInfo, layerIndex);
        _rigidbody = animator.GetComponent<Rigidbody>();
        Jump(animator);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MoveAndRotate(animator, legendController.GetMoveDirection());
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

    private void MoveAndRotate(Animator animator, Vector3 forward)
    {
        if (forward == Vector3.zero)
        {
            return;
        }

        animator.transform.forward = forward;
        animator.transform.Translate(_moveDirection * (5.3f * Time.deltaTime));
    }
}

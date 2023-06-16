using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookJumpAttackState : StateMachineBehaviour
{
    private PlayerMove _playerMove;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        animator.SetBool(AnimationHash.JumpDown, true);

        _playerMove = animator.GetComponent<PlayerMove>();

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playerMove.moveDirection != Vector3.zero)
        {
            animator.transform.forward = _playerMove.moveDirection;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(AnimationHash.JumpAttack);
    }

}

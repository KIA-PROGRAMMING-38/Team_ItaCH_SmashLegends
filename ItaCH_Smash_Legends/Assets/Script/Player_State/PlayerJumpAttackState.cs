using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpAttackState : StateMachineBehaviour
{
    private PlayerJump _playerJump;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(AnimationHash.JumpDown, true);
        _playerJump = animator.GetComponent<PlayerJump>();
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

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpState : StateMachineBehaviour
{
    private PlayerJump _playerJump;
    private Rigidbody _rigidbody;
    private PlayerStatus _playerStatus;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerJump = animator.GetComponent<PlayerJump>();
        _rigidbody = animator.GetComponent<Rigidbody>();
        _playerStatus = animator.GetComponent<PlayerStatus>();
        
        _playerStatus.IsJump = false;
        _playerStatus.CurrentState = PlayerStatus.State.Jump;

        animator.SetBool(AnimationHash.Run, false);

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerJump.JumpMoveAndRotate();
        if (_rigidbody.velocity.y <= -1)
        {
            animator.SetBool(AnimationHash.JumpDown, true);
        }
    }
}

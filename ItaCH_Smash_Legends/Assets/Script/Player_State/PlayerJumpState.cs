using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpState : StateMachineBehaviour
{
    private PlayerJump _playerJump;
    private PlayerMove _playerMove;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerJump = animator.GetComponent<PlayerJump>();
        _playerMove = animator.GetComponent<PlayerMove>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerMove.MoveAndRotate();
        if(_playerJump._rigidbody.velocity.y <= 0f)
        {
            animator.SetBool(AnimationHash.JumpDown, true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


}

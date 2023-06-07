using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpDownState : StateMachineBehaviour
{
    private PlayerMove _playerMove;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerMove = animator.GetComponent<PlayerMove>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.3f)
        _playerMove.MoveAndRotate();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}

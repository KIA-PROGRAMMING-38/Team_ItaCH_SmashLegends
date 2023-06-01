using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : StateMachineBehaviour
{
    private PlayerMove _playerMove; 
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerMove = animator.GetComponent<PlayerMove>();
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerMove.MoveAndRotate();
    }

}

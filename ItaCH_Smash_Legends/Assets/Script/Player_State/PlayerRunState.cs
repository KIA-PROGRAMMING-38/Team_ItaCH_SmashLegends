using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : StateMachineBehaviour
{
    private PlayerMove _playerMove;
    private PlayerAttack _playerAttack;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerMove = animator.GetComponent<PlayerMove>();
        _playerAttack = animator.GetComponent<PlayerAttack>();
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerMove.MoveAndRotate();
        if (_playerAttack.isAttack)
        {
            animator.SetBool(AnimationHash.Run, false);
            _playerAttack.isAttack = false;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}

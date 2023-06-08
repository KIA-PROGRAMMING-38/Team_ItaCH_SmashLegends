using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : StateMachineBehaviour
{
    private PlayerStatus _playerStatus;
    private PlayerMove _playerMove;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerMove = animator.GetComponent<PlayerMove>();
        _playerStatus = animator.GetComponent<PlayerStatus>();

        _playerStatus.CurrentState = PlayerStatus.State.Idle;

        if(_playerMove.moveDirection != Vector3.zero)
        {
            animator.SetBool(AnimationHash.Run, true);
        }
    }



}

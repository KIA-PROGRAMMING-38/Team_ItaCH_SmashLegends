using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDownIdleState : StateMachineBehaviour
{
    PlayerStatus _playerStatus;
    PlayerRollUp _playerRollUp;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerRollUp = animator.GetComponent<PlayerRollUp>();
        _playerStatus = animator.GetComponent<PlayerStatus>();

        _playerStatus.CurrentState = PlayerStatus.State.DownIdle;
        _playerStatus.IsRollUp = true;
        _playerRollUp.RoiingInputWaitTask().Forget();
    }
}

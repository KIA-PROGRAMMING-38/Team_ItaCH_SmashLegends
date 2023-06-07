using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavyAttackState : StateMachineBehaviour
{
    PlayerAttack _playerAttack;
    PlayerStatus _playerStatus;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack = animator.GetComponent<PlayerAttack>();
        _playerStatus = animator.GetComponent<PlayerStatus>();

        _playerAttack.AttackOnDefaultDash();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerStatus.CurrentState = PlayerStatus.State.Idle;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFinishAttackState : StateMachineBehaviour
{
    private PlayerAttack _playerAttack;
    private PlayerInput _playerInput;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack = animator.GetComponent<PlayerAttack>();
        _playerAttack.AttackOnDefaultDash();
        _playerInput = animator.GetComponent<PlayerInput>();
        _playerAttack.isFinishAttack = false;
        _playerAttack.CurrentPossibleComboCount = _playerAttack.MAX_POSSIBLE_ATTACK_COUNT;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


}

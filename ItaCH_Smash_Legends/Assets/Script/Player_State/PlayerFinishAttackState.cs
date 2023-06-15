using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFinishAttackState : StateMachineBehaviour
{
    private PlayerAttack _playerAttack;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack = animator.GetComponent<PlayerAttack>();

        --_playerAttack.CurrentPossibleComboCount;
        _playerAttack.isSecondAttack = false;
        _playerAttack.isFinishAttack = false;
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack.CurrentPossibleComboCount = _playerAttack.MAX_POSSIBLE_ATTACK_COUNT;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFinishAttackState : StateMachineBehaviour
{
    private PlayerAttack _playerAttack;
    private PlayerStatus _playerStatus;
    private PlayerHit _playerHit;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack = animator.GetComponent<PlayerAttack>();
        _playerStatus = animator.GetComponent<PlayerStatus>();
        _playerHit = animator.GetComponent<PlayerHit>();

        _playerAttack.AttackOnDash();
        //_playerHit.AttackRangeOn();

        --_playerAttack.CurrentPossibleComboCount;
        _playerAttack.isSecondAttack = false;
        _playerAttack.isFinishAttack = false;
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack.CurrentPossibleComboCount = _playerAttack.MAX_POSSIBLE_ATTACK_COUNT;
        //_playerHit.AttackRangeOff();
    }
}

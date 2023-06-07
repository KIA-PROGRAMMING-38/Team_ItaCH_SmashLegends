using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFinishAttackState : StateMachineBehaviour
{
    private PlayerAttack _playerAttack;
    private PlayerStatus _playerStatus;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack = animator.GetComponent<PlayerAttack>();
        _playerAttack.AttackOnDefaultDash();
        _playerStatus = animator.GetComponent<PlayerStatus>();
        --_playerAttack.CurrentPossibleComboCount;
        _playerAttack.isSecondAttack = false;
        _playerAttack.isFinishAttack = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack.CurrentPossibleComboCount = _playerAttack.MAX_POSSIBLE_ATTACK_COUNT;
        if (_playerStatus.CurrentState == PlayerStatus.State.Run)
        {
            animator.SetBool(AnimationHash.Run, true);
        }
        else
        {
            animator.Play(AnimationHash.Idle);
        }
        Debug.Log(_playerStatus.CurrentState);
    }
}

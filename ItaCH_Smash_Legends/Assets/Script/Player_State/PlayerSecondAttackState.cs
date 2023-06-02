using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSecondAttackState : StateMachineBehaviour
{
    private PlayerAttack _playerAttack;
    private PlayerInput _playerInput;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack = animator.GetComponent<PlayerAttack>();
        _playerAttack.AttackOnDefaultDash();
        _playerInput = animator.GetComponent<PlayerInput>();
        --_playerAttack.CurrentPossibleComboCount;
        _playerAttack.isFirstAttack = false;
        _playerAttack.isSecondAttack = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_playerAttack.isFinishAttack && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.7f)
        {
            animator.SetBool(AnimationHash.SecondAttack, false);
            animator.SetTrigger(AnimationHash.FinishAttack);
        }
        else if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
        {
            animator.SetBool(AnimationHash.SecondAttack, false);
            _playerAttack.CurrentPossibleComboCount = _playerAttack.MAX_POSSIBLE_ATTACK_COUNT;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


}

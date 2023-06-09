using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavyAttackState : StateMachineBehaviour
{
    private PlayerAttack _playerAttack;
    private PlayerHit _playerHit;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack = animator.GetComponent<PlayerAttack>();
        _playerHit = animator.GetComponent<PlayerHit>();
        animator.SetBool(AnimationHash.Run, false);

        _playerAttack.AttackOnDefaultDash();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack.AttackRotate();
        _playerHit.AttackRangeOff();

    }

}

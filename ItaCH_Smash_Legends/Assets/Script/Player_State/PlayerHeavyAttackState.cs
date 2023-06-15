using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavyAttackState : StateMachineBehaviour
{
    private PlayerAttack _playerAttack;    

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack = animator.GetComponent<PlayerAttack>();
        animator.SetBool(AnimationHash.Run, false);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack.AttackRotate();
    }

}

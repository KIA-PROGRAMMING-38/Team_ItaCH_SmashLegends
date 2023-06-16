using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookJumpLandState : StateMachineBehaviour
{
    private PlayerAttack _playerAttack;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack = animator.GetComponent<PlayerAttack>();
        _playerAttack.CurrentPossibleComboCount = _playerAttack.MAX_POSSIBLE_ATTACK_COUNT;
        animator.ResetTrigger(AnimationHash.JumpAttack);
        animator.SetBool(AnimationHash.JumpSecondAttack, false);
        animator.SetBool(AnimationHash.JumpFinishAttack, false);


    }

}

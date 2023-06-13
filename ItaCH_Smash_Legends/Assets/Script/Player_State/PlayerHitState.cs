using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHitState : StateMachineBehaviour
{
    private PlayerHit _playerHit;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerHit = animator.GetComponent<PlayerHit>();
        
        _playerHit.invincible = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerHit.invincible = false;
        animator.ResetTrigger(AnimationHash.Hit);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitUpState : StateMachineBehaviour
{
    private Rigidbody _rigidbody;
    private PlayerHit _playerHit;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _rigidbody = animator.GetComponent<Rigidbody>();
        _playerHit = animator.GetComponent<PlayerHit>();
        _playerHit.invincible = true;

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_rigidbody.velocity.y <= 0)
        {
            animator.SetTrigger(AnimationHash.HitDown);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerHit.invincible = false;
        animator.ResetTrigger(AnimationHash.HitDown);
    }


}

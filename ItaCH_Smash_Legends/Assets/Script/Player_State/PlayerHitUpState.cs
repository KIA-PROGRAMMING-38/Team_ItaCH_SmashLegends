using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHitUpState : StateMachineBehaviour
{
    private PlayerHit _playerHit;
    private PlayerStatus _playerStatus;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerHit = animator.GetComponent<PlayerHit>();
        _playerStatus = animator.GetComponent<PlayerStatus>();
        _playerStatus.CurrentState = PlayerStatus.State.HitUp;
        Debug.Log("HitUp 들어옴");
        _playerHit.invincible = true;
    }
    // 추후 수정
    //public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6)
    //    {
    //    }
    //}
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerStatus.CurrentState = PlayerStatus.State.Idle;
        _playerHit.invincible = false;
        animator.ResetTrigger(AnimationHash.HitUp);
    }
}

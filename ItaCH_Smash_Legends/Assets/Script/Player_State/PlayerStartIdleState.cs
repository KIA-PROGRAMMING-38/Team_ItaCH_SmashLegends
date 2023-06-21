using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartIdleState : StateMachineBehaviour
{
    private PlayerHit _playerHit;
    private float _elapsedTime = 0f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerHit = animator.GetComponent<PlayerHit>();
        _playerHit.invincible = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InvincibleTime();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    private void InvincibleTime()
    {
        float delta = Time.deltaTime;
        _elapsedTime += delta;
        Debug.Log(_elapsedTime);
        if(_elapsedTime > 2.1f)
        {
            Debug.Log("무적 풀림");
            _playerHit.invincible = false;
        }
    }
}

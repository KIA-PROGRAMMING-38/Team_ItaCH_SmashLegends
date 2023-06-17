using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerSkillAttackState : StateMachineBehaviour
{
    private float _moveSpeed = 5f;

    private PlayerStatus _playerStatus;
    private PlayerHit _playerHit;
    private Transform _transform;
    //private Rigidbody _rigidbody;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerStatus = animator.GetComponent<PlayerStatus>();
        _playerHit = animator.GetComponent<PlayerHit>();
        _transform = animator.GetComponent<Transform>();
        //_rigidbody = animator.GetComponent<Rigidbody>();
        _playerStatus.CurrentState = PlayerStatus.State.SkillAttack;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //_rigidbody.velocity = _transform.forward * _moveSpeed;
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
        {
            // Peter Lagacy Code 및 추가 이슈에서 수정
            //_rigidbody.velocity = Vector3.zero;
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.35f)
        {
            _playerStatus.CurrentState = PlayerStatus.State.SkillEndAttack;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}

using System.Linq.Expressions;
using UnityEngine;

public class PlayerFirstAttackState : StateMachineBehaviour
{
    private PlayerAttack _playerAttack;
    private PlayerInput _playerInput;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack = animator.GetComponent<PlayerAttack>();
        _playerAttack.AttackOnDefaultDash();
        _playerInput = animator.GetComponent<PlayerInput>();

        --_playerAttack.CurrentPossibleComboCount;
        _playerAttack.isFirstAttack = true;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_playerAttack.isSecondAttack && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.8f)
        {
            animator.SetBool(AnimationHash.FirstAttack, false);
            animator.SetBool(AnimationHash.SecondAttack, true);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            animator.SetBool(AnimationHash.FirstAttack, false);
            _playerAttack.CurrentPossibleComboCount = _playerAttack.MAX_POSSIBLE_ATTACK_COUNT;
            _playerAttack.isFirstAttack = false;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}

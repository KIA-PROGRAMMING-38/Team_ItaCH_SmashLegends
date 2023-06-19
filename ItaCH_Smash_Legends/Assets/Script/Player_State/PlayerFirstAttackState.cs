using UnityEngine;

public class PlayerFirstAttackState : StateMachineBehaviour
{
    private PlayerAttack _playerAttack;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack = animator.GetComponent<PlayerAttack>();
        animator.SetBool(AnimationHash.Run, false);

        --_playerAttack.CurrentPossibleComboCount;
        _playerAttack.isFirstAttack = true;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playerAttack.isSecondAttack && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= _playerAttack.nextTransitionMinValue &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= _playerAttack.nextTransitionMaxValue)
        {
            _playerAttack.AttackRotate();
            animator.Play(AnimationHash.SecondAttack);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= _playerAttack.nextTransitionMaxValue)
        {
            _playerAttack.CurrentPossibleComboCount = _playerAttack.MAX_POSSIBLE_ATTACK_COUNT;
            _playerAttack.isFirstAttack = false;
        }
    }

}

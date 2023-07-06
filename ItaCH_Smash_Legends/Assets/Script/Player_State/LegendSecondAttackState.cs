using UnityEngine;

public class LegendSecondAttackState : LegendBaseState
{
    // LegnedController 완료시 리펙토링

    private PlayerAttack _playerAttack;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        _playerAttack = animator.GetComponent<PlayerAttack>();

        --_playerAttack.CurrentPossibleComboCount;
        _playerAttack.isFirstAttack = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playerAttack.isFinishAttack && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.7f)
        {
            _playerAttack.AttackRotate();
            animator.Play(AnimationHash.FinishAttack);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
        {
            _playerAttack.CurrentPossibleComboCount = _playerAttack.MAX_POSSIBLE_ATTACK_COUNT;
            _playerAttack.isSecondAttack = false;
        }
    }
}
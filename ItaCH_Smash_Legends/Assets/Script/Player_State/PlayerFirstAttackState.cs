using UnityEngine;

public class PlayerFirstAttackState : StateMachineBehaviour
{
    private PlayerAttack _playerAttack;
    private LegendController _legendController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _legendController = animator.GetComponent<LegendController>();
        _playerAttack = animator.GetComponent<PlayerAttack>();
        animator.SetBool(AnimationHash.Run, false);

        --_playerAttack.CurrentPossibleComboCount;
        _playerAttack.isFirstAttack = true;
        _legendController.NextPlayClip();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //if (_playerAttack.isFinishAttack && _playerAttack.CurrentPossibleComboCount == _playerAttack.COMBO_FINISH_COUNT)
        //{
        //    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= _playerAttack.nextTransitionMinValue)
        //    {
        //        _playerAttack.AttackRotate();
        //        animator.Play(AnimationHash.FinishAttack);
        //    }
        //}
        if (/*_playerAttack.isSecondAttack && */animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= _playerAttack.nextTransitionMinValue &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= _playerAttack.nextTransitionMaxValue)
        {
            //_playerAttack.AttackRotate();
            _legendController.PlaySecondAttack();
            //animator.Play(AnimationHash.SecondAttack);
        }
        //else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= _playerAttack.nextTransitionMaxValue)
        //{
        //    _playerAttack.CurrentPossibleComboCount = _playerAttack.MAX_POSSIBLE_ATTACK_COUNT;
        //    _playerAttack.isFirstAttack = false;
        //}

    }
}
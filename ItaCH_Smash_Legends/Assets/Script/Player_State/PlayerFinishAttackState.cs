using UnityEngine;

public class PlayerFinishAttackState : StateMachineBehaviour
{
    private PlayerAttack _playerAttack;
    private PlayerStatus _playerStatus;
    private LegendController _legendController;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack = animator.GetComponent<PlayerAttack>();
        _playerStatus = animator.GetComponent<PlayerStatus>();
        _legendController= animator.GetComponent<LegendController>();

        _playerStatus.CurrentState = PlayerStatus.State.FinishComboAttack;
        --_playerAttack.CurrentPossibleComboCount;
        _playerAttack.isSecondAttack = false;
        _playerAttack.isFinishAttack = false;
        _legendController.NextPlayClip();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (/*_playerAttack.isSecondAttack && */animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= _playerAttack.nextTransitionMinValue &&
           animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= _playerAttack.nextTransitionMaxValue)
        {
            //_playerAttack.AttackRotate();
            _legendController.PlayFirstAttack();
            //animator.Play(AnimationHash.SecondAttack);
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack.CurrentPossibleComboCount = _playerAttack.MAX_POSSIBLE_ATTACK_COUNT;
        _legendController.PossibleComboCount = 0;
    }
}

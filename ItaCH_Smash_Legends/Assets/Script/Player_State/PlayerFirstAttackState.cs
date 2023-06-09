using UnityEngine;

public class PlayerFirstAttackState : StateMachineBehaviour
{
    private PlayerAttack _playerAttack;
    private PlayerStatus _playerStatus;
    private PlayerHit _playerHit;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack = animator.GetComponent<PlayerAttack>();
        _playerStatus = animator.GetComponent<PlayerStatus>();
        _playerHit = animator.GetComponent<PlayerHit>();
        animator.SetBool(AnimationHash.Run, false);
        _playerStatus.CurrentState = PlayerStatus.State.ComboAttack;
        _playerAttack.AttackOnDefaultDash();

        --_playerAttack.CurrentPossibleComboCount;
        _playerAttack.isFirstAttack = true;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playerAttack.isSecondAttack && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.8f)
        {
            _playerAttack.AttackRotate();
            animator.Play(AnimationHash.SecondAttack);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
        {
            _playerAttack.CurrentPossibleComboCount = _playerAttack.MAX_POSSIBLE_ATTACK_COUNT;
            _playerAttack.isFirstAttack = false;
            _playerHit.AttackRangeOff();
        }
    }

}

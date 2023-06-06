using UnityEngine;

public class PlayerFirstAttackState : StateMachineBehaviour
{
    private PlayerAttack _playerAttack;
    private PlayerStatus _playerStatus;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack = animator.GetComponent<PlayerAttack>();
        _playerAttack.AttackOnDefaultDash();
        _playerStatus = animator.GetComponent<PlayerStatus>();

        --_playerAttack.CurrentPossibleComboCount;
        _playerAttack.isFirstAttack = true;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_playerAttack.isSecondAttack && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.8f)
        {
            animator.Play(AnimationHash.SecondAttack);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
        {
            _playerAttack.CurrentPossibleComboCount = _playerAttack.MAX_POSSIBLE_ATTACK_COUNT;
            _playerAttack.isFirstAttack = false;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playerStatus.CurrentState == PlayerStatus.State.Run)
        {
            animator.SetBool(AnimationHash.Run, true);
        }
        else
        {
            animator.Play(AnimationHash.Idle);
        }
        Debug.Log(_playerStatus.CurrentState);
    }
}

using UnityEngine;

public class PlayerIdleState : StateMachineBehaviour
{
    private PlayerStatus _playerStatus;
    private PlayerMove _playerMove;
    private PlayerHit _playerHit;
    private PlayerAttack _playerAttack;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerMove = animator.GetComponent<PlayerMove>();
        _playerStatus = animator.GetComponent<PlayerStatus>();
        _playerHit = animator.GetComponent<PlayerHit>();
        _playerAttack = animator.GetComponent<PlayerAttack>();

        _playerStatus.CurrentState = PlayerStatus.State.Idle;
        animator.ResetTrigger(AnimationHash.RollUpBack);
        animator.ResetTrigger(AnimationHash.RollUpFront);
        _playerStatus.IsHang = false;

        _playerAttack.CurrentPossibleComboCount = _playerAttack.MAX_POSSIBLE_ATTACK_COUNT;
        if (_playerMove.moveDirection != Vector3.zero)
        {
            animator.SetBool(AnimationHash.Run, true);
        }
    }
}

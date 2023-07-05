using UnityEngine;

public class PlayerIdleState : StateMachineBehaviour
{
    private PlayerStatus _playerStatus;
    private PlayerMove _playerMove;
    private PlayerAttack _playerAttack;
    private LegendController _legendController;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerMove = animator.GetComponent<PlayerMove>();
        _playerStatus = animator.GetComponent<PlayerStatus>();
        _playerAttack = animator.GetComponent<PlayerAttack>();
        _legendController = animator.GetComponent<LegendController>();

        _playerStatus.CurrentState = PlayerStatus.State.Idle;
        animator.ResetTrigger(AnimationHash.RollUpBack);
        animator.ResetTrigger(AnimationHash.RollUpFront);
        _playerStatus.IsHang = false;

        _playerAttack.CurrentPossibleComboCount = _playerAttack.MAX_POSSIBLE_ATTACK_COUNT;
        _legendController.PossibleComboCount = 0;
        if (_playerMove.moveDirection != Vector3.zero)
        {
            animator.SetBool(AnimationHash.Run, true);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _legendController.NextAnimation();
    }
}

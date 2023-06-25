using UnityEngine;

public class PlayerRunState : StateMachineBehaviour
{
    private PlayerMove _playerMove;
    private PlayerAttack _playerAttack;
    private PlayerStatus _playerStatus;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerStatus = animator.GetComponent<PlayerStatus>();
        _playerMove = animator.GetComponent<PlayerMove>();
        _playerAttack = animator.GetComponent<PlayerAttack>();
        _playerStatus.CurrentState = PlayerStatus.State.Run;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerMove.MoveAndRotate();
    }
}

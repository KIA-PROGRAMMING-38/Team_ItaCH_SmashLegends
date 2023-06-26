using UnityEngine;

public class PlayerRunState : StateMachineBehaviour
{
    private PlayerMove _playerMove;
    private PlayerStatus _playerStatus;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerStatus = animator.GetComponent<PlayerStatus>();
        _playerMove = animator.GetComponent<PlayerMove>();
        _playerStatus.CurrentState = PlayerStatus.State.Run;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerMove.MoveAndRotate();
    }
}

using UnityEngine;

public class PlayerHangState : StateMachineBehaviour
{
    private PlayerHangController _playerHangController;
    private PlayerStatus _playerStatus;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerHangController = animator.GetComponent<PlayerHangController>();
        _playerStatus = animator.GetComponent<PlayerStatus>();

        _playerStatus.CurrentState = PlayerStatus.State.Hang;

        animator.SetBool(AnimationHash.Run, false);
        _playerHangController.OnFalling().Forget();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerHangController.OffConstraints();
        _playerHangController.TaskCancel.Cancel();
    }
}

using UnityEngine;

public class LegendDownIdleState : LegendBaseState
{

    // LegnedController 완료시 리펙토링

    PlayerStatus _playerStatus;
    PlayerRollUp _playerRollUp;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _playerRollUp = animator.GetComponent<PlayerRollUp>();
        _playerStatus = animator.GetComponent<PlayerStatus>();

        _playerStatus.CurrentState = PlayerStatus.State.DownIdle;
        _playerStatus.IsRollUp = true;
        _playerRollUp.RoiingInputWaitTask().Forget();
    }
}

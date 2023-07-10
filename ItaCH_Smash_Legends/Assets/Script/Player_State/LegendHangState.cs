using UnityEngine;

public class LegendHangState : LegendBaseState
{
    // LegnedController 완료시 리펙토링

    private PlayerHangController _playerHangController;
    private PlayerStatus _playerStatus;
    private EffectController _effectController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        _effectController = animator.GetComponent<EffectController>();
        _playerHangController = animator.GetComponent<PlayerHangController>();
        _playerStatus = animator.GetComponent<PlayerStatus>();

        _playerStatus.CurrentState = PlayerStatus.State.Hang;

        animator.SetBool(AnimationHash.Run, false);
        _playerHangController.OnFalling().Forget();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerHangController.OffConstraints();
        _effectController.StartInvincibleFlashEffet(_effectController.HANG_JUMP_FLASH_COUNT).Forget();
        _playerHangController.TaskCancel.Cancel();
    }
}

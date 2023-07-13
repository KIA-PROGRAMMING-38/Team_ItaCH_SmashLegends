using UnityEngine;

public class LegendHangState : LegendBaseState
{
    private EffectController _effectController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _effectController = animator.GetComponent<EffectController>();

        legendController.OnFalling(animator).Forget();
        _effectController.StartInvincibleFlashEffet(_effectController.FLASH_COUNT).Forget();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (legendController.IsTriggered(ActionType.Jump))
        {
            animator.Play(AnimationHash.Jump);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        legendController.OffConstraints();
        _effectController.StartInvincibleFlashEffet(_effectController.HANG_JUMP_FLASH_COUNT).Forget();
        legendController.TaskCancel.Cancel();
    }
}

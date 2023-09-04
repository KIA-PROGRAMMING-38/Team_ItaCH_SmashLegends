using UnityEngine;

public class LegendHangState : LegendBaseState
{
    private EffectController _effectController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _effectController = animator.GetComponent<EffectController>();

        legendController.FallAsync(animator).Forget();
        _effectController.StartInvincibleFlashEffet(_effectController.FLASH_COUNT).Forget();
        Managers.SoundManager.Play(SoundType.SFX, StringLiteral.SFX_HANG, legendController.LegendType);
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
        legendController.EscapeInHang();
        _effectController.StartInvincibleFlashEffet(_effectController.HANG_JUMP_FLASH_COUNT).Forget();
    }
}

using UnityEngine;

public class LegendRollUpFrontState : LegendBaseState
{
    private EffectController _effectController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        
        _effectController = animator.GetComponent<EffectController>();

        legendController.DashOnRollUp();
        Managers.SoundManager.Play(SoundType.SFX,StringLiteral.SFX_ROLLFRONT,legendController.LegendType);
        _effectController.StartInvincibleFlashEffet(_effectController.FLASH_COUNT).Forget();
    }
}

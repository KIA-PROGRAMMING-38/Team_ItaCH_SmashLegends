using UnityEngine;

public class LegendHitState : LegendBaseState
{
    private EffectController _effectController;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _effectController = animator.GetComponent<EffectController>();
        _effectController.StartHitFlashEffet().Forget();
    }
}

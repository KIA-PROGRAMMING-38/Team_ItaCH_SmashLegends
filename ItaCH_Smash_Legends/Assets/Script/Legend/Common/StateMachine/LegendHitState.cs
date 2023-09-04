using UnityEngine;

public class LegendHitState : LegendBaseState
{
    private EffectController _effectController;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        _effectController = animator.GetComponent<EffectController>();
        _effectController.StartHitFlashEffet().Forget();
        Managers.SoundManager.Play(SoundType.Voice,legend: legendController.LegendType, voice: VoiceType.Hit);
    }
}

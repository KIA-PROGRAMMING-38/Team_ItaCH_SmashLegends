using UnityEngine;

public class LegendHitUpState : LegendBaseState
{
    private EffectController _effectController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _effectController = animator.GetComponent<EffectController>();
        _effectController.StartHitFlashEffet().Forget();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (legendController.IsFallingOnHitUp())
        {
            if (legendController.IsTriggered(ActionType.Jump))
            {
                legendController.ResetVelocity();
                animator.Play(AnimationHash.Jump);
            }
        }
    }
}

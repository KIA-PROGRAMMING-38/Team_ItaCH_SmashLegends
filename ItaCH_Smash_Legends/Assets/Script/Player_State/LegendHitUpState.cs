using UnityEngine;

public class LegendHitUpState : LegendBaseState
{
    // LegnedController 완료시 리펙토링

    private PlayerHit _playerHit;
    private PlayerStatus _playerStatus;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        _playerHit = animator.GetComponent<PlayerHit>();
        _playerStatus = animator.GetComponent<PlayerStatus>();
        _playerStatus.CurrentState = PlayerStatus.State.HitUp;
    }
    // 추후 수정
    //public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6)
    //    {
    //    }
    //}
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerStatus.CurrentState = PlayerStatus.State.Idle;
        animator.ResetTrigger(AnimationHash.HitUp);
    }
}

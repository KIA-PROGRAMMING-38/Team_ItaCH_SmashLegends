using UnityEngine;

public class LegendHeavyAttackState : LegendBaseState
{
    // LegnedController 완료시 리펙토링
   
    private PlayerAttack _playerAttack;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        _playerAttack = animator.GetComponent<PlayerAttack>();
        animator.SetBool(AnimationHash.Run, false);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack.AttackRotate();
    }

}

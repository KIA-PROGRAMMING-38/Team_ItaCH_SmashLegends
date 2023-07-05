using UnityEngine;

public class PlayerFirstAttackState : StateMachineBehaviour
{
    private LegendController _legendController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _legendController = animator.GetComponent<LegendController>();

        _legendController.NextComboImPossible();
        _legendController.PlayNextClip();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_legendController.SetAttackable())
        {
            _legendController.PlaySecondAttack();
        }
    }
}
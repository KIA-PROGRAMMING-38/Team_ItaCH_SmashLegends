using UnityEngine;

public class PlayerFinishAttackState : StateMachineBehaviour
{
    private LegendController _legendController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _legendController = animator.GetComponent<LegendController>();

        _legendController.SetComboImPossible();
        _legendController.PlayNextClip();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_legendController.SetAttackable())
        {
            _legendController.PlayFirstAttack();
        }
    }
}

using UnityEngine;

public class LegendBaseState : StateMachineBehaviour
{
    protected LegendController legendController;
    protected LegendAnimationController legendAnimationController;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        legendAnimationController = animator.GetComponent<LegendAnimationController>();
        legendController = animator.GetComponent<LegendController>();
    }
}

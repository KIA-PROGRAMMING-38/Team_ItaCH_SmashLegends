using UnityEngine;

public class PlayerSkillAttackState : StateMachineBehaviour
{
    private PlayerStatus _playerStatus;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerStatus = animator.GetComponent<PlayerStatus>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.35f)
        {
            _playerStatus.CurrentState = PlayerStatus.State.SkillEndAttack;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}

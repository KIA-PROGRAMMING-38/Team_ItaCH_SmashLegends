using UnityEngine;

public class PlayerSkillAttackState : StateMachineBehaviour
{
    private PlayerStatus _playerStatus;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerStatus = animator.GetComponent<PlayerStatus>();
        _playerStatus.CurrentState = PlayerStatus.State.SkillAttack;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHangState : StateMachineBehaviour
{
    private PlayerHangController _playerHangController;
    private PlayerStatus _playerStatus;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerHangController = animator.GetComponent<PlayerHangController>();
        _playerStatus = animator.GetComponent<PlayerStatus>();

        _playerStatus.IsJump = false;

        _playerHangController.StartCoroutine(_playerHangController.WaitFallingCoroutine);
    }
  
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerHangController.OffConstraints();
        _playerStatus.IsHang = false;
        _playerHangController.StopCoroutine(_playerHangController.WaitFallingCoroutine);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliceAttack : PlayerAttack
{
    [SerializeField] private GameObject _aliceBomb;
    private void Start()
    {
        CurrentPossibleComboCount = MAX_POSSIBLE_ATTACK_COUNT;
    }
    
    public override void DefaultAttack()
    {
        if (IsPossibleFirstAttack())
        {
            animator.Play(AnimationHash.FirstAttack);
            --CurrentPossibleComboCount;
        }
        if (isFirstAttack && CurrentPossibleComboCount == COMBO_FINISH_COUNT)
        {
            isSecondAttack = true;
        }
    }

    public override void HeavyAttack()
    {
        if (playerStatus.CurrentState == PlayerStatus.State.Run ||
           playerStatus.CurrentState == PlayerStatus.State.Idle)
        {
            animator.Play(AnimationHash.HeavyAttack);
            playerStatus.CurrentState = PlayerStatus.State.HeavyAttack;
        }
    }

    private void HeavyAttackBomb() => _aliceBomb.gameObject.SetActive(true);
}

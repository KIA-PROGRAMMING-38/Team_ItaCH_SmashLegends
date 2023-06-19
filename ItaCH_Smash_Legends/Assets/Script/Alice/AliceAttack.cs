using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AliceAttack : PlayerAttack
{
    [SerializeField] private GameObject _aliceBomb;
    internal Rigidbody _rigidbody; 
    private void Start()
    {
        CurrentPossibleComboCount = MAX_POSSIBLE_ATTACK_COUNT;
        _rigidbody = GetComponent<Rigidbody>();
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

    public override void SkillAttack()
    {
        if (playerStatus.CurrentState == PlayerStatus.State.Run ||
            playerStatus.CurrentState == PlayerStatus.State.Idle ||
            playerStatus.CurrentState == PlayerStatus.State.Jump)
        {
            Vector3 direction = transform.forward + transform.up;
            _rigidbody.AddForce(direction * 0.85f, ForceMode.Impulse);
            animator.Play(AnimationHash.SkillAttack);
            playerStatus.CurrentState = PlayerStatus.State.SkillAttack;
        }
    }
    private void HeavyAttackBomb() => _aliceBomb.gameObject.SetActive(true);
}

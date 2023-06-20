using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static AliceEffectController;

public class AliceAttack : PlayerAttack
{
    [SerializeField] private GameObject _aliceBomb;
    private Rigidbody _rigidbody;
    private AliceEffectController _aliceEffectController;
    private void Start()
    {
        CurrentPossibleComboCount = MAX_POSSIBLE_ATTACK_COUNT;
        _rigidbody = GetComponent<Rigidbody>();
        _aliceEffectController = GetComponent<AliceEffectController>();
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
        if (playerStatus.CurrentState == PlayerStatus.State.Run 
            || playerStatus.CurrentState == PlayerStatus.State.Idle)
            
        {
            Vector3 direction = (transform.forward + transform.up).normalized;
            _rigidbody.AddForce(direction * 1f, ForceMode.Impulse);
            animator.Play(AnimationHash.SkillAttack);
            playerStatus.CurrentState = PlayerStatus.State.SkillAttack;
        }
    }

    private void HeavyAttackBomb() => _aliceBomb.gameObject.SetActive(true);
}

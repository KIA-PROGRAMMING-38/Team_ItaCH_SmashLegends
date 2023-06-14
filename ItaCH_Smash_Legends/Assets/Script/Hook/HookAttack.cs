using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class HookAttack : PlayerAttack
{
    private PlayerStatus _playerStatus;
    private Animator _animator;
    public Transform _bulletSpawnPositionLeft;
    public Transform _bulletSpawnPositionRight;
    public HookBullet _hookBullet;
    private void Start()
    {
        _playerStatus= GetComponent<PlayerStatus>();
         //_bulletSpawnPosition = GetComponent<Transform>().GetChild(0);
    }
    public override void AttackOnDash()
    {
        if (CurrentPossibleComboCount == COMBO_SECOND_COUNT)
        {
            rigidbodyAttack.AddForce(transform.forward * (_defaultDashPower * -0.4f), ForceMode.Impulse);
            
        }
        if(CurrentPossibleComboCount == MAX_POSSIBLE_ATTACK_COUNT && 
            _playerStatus.CurrentState == PlayerStatus.State.HeavyAttack)
        {
            rigidbodyAttack.AddForce(transform.forward * (_defaultDashPower * -0.65f), ForceMode.Impulse);
        }
    }

    public override void DefaultAttack()
    {
       
    }

    public override void HeavyAttack()
    {
        base.HeavyAttack();
    }

    public override void JumpAttack()
    {
        _animator.SetTrigger(AnimationHash.JumpAttack);
    }

    public override void SkillAttack()
    {
        base.SkillAttack();
    }

    public void DefaultAttackLeft()
    {
        HookBullet bullet = Instantiate(_hookBullet, _bulletSpawnPositionLeft.position, Quaternion.identity);
        bullet.gameObject.SetActive(true);
        bullet.transform.forward = transform.forward;
    }
    public void DefaultAttackRight()
    {
        HookBullet bullet = Instantiate(_hookBullet, _bulletSpawnPositionRight.position, Quaternion.identity);
        bullet.gameObject.SetActive(true);
        bullet.transform.forward = transform.forward;
    }
}

interface IObjectPool
{

}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TreeEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

public class HookAttack : PlayerAttack
{
    private PlayerStatus _playerStatus;
    private Animator _animator;
    public Transform _bulletSpawnPositionLeft;
    public Transform _bulletSpawnPositionRight;
    public GameObject _hookEffectContainer;

    private HookBullet _hookBullet;
    private GameObject _bulletCreateEffect;

    private void Start()
    {
        _playerStatus = GetComponent<PlayerStatus>();
        _hookBullet = _hookEffectContainer.transform.GetChild(0).GetComponent<HookBullet>();
        _bulletCreateEffect = _hookEffectContainer.transform.GetChild(2).gameObject;
    }
    public override void AttackOnDash()
    {
        if (CurrentPossibleComboCount == COMBO_SECOND_COUNT)
        {
            rigidbodyAttack.AddForce(transform.forward * (_defaultDashPower * -0.4f), ForceMode.Impulse);

        }
        if (CurrentPossibleComboCount == MAX_POSSIBLE_ATTACK_COUNT &&
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
        CreateBullet(_bulletSpawnPositionLeft);
    }

    public void DefaultAttackRight()
    {
        CreateBullet(_bulletSpawnPositionRight);
    }

    private void CreateBullet(Transform spawnPosition)
    {
        GameObject effect = Instantiate(_bulletCreateEffect, spawnPosition.position, _bulletCreateEffect.transform.rotation);
        HookBullet bullet = Instantiate(_hookBullet, spawnPosition.position, Quaternion.identity);
        SetEffectRotate(effect);

        bullet.gameObject.SetActive(true);
        bullet.transform.forward = transform.forward;
    }
    private void SetEffectRotate(GameObject effect)
    {
        if (transform.rotation.eulerAngles.y == 0)
        {
            effect.transform.rotation = Quaternion.Euler(-90, 0, 0);

        }
        else if (transform.rotation.eulerAngles.y == 45)
        {
            effect.transform.rotation = Quaternion.Euler(-90, 45, 0);

        }
        else if (transform.rotation.eulerAngles.y == 90)
        {
            effect.transform.rotation = Quaternion.Euler(0, 0, 90);

        }
        else if (transform.rotation.eulerAngles.y == 135)
        {

            effect.transform.rotation = Quaternion.Euler(90, -45, 0);
        }
        else if (transform.rotation.eulerAngles.y == 180)
        {
            effect.transform.rotation = Quaternion.Euler(90, 0, 0);

        }
        else if (transform.rotation.eulerAngles.y == 225)
        {
            effect.transform.rotation = Quaternion.Euler(90, 45, 0);

        }
        else if (transform.rotation.eulerAngles.y == 270)
        {
            effect.transform.rotation = Quaternion.Euler(0, 0, -90);

        }
        else if (transform.rotation.eulerAngles.y == 315)
        {
            effect.transform.rotation = Quaternion.Euler(0, 45, -90);

        }


    }
}

interface IObjectPool
{

}

using System;
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

    private Transform _bulletSpawnPositionLeft;
    private Transform _bulletSpawnPositionRight;

    private GameObject[] _bulletContainers = new GameObject[3];

    private HookBullet[] _bullets = new HookBullet[3];
    private GameObject[] _bulletCreateEffect = new GameObject[3];

    private int _defalutIndex = 0;
    private int _heavyIndex = 1;
    private int _lastHeavyIndex = 2;

    private void Start()
    {
        _playerStatus = GetComponent<PlayerStatus>();
        _bulletSpawnPositionLeft = transform.GetChild(5).GetChild(0).GetChild(2).GetChild(0).transform;
        _bulletSpawnPositionRight = transform.GetChild(5).GetChild(0).GetChild(3).GetChild(0).transform;

        for (int i = 0; i < _bulletContainers.Length; ++i)
        {
            _bulletContainers[i] = transform.GetChild(i).gameObject;
            _bullets[i] = _bulletContainers[i].transform.GetChild(0).GetComponent<HookBullet>();
            _bulletCreateEffect[i] = _bulletContainers[i].transform.GetChild(1).gameObject;
        }

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
        CreateBullet(_bulletSpawnPositionLeft.position, _bullets[_defalutIndex], _defalutIndex);
        CreateDefaultBulletEffect(_bulletSpawnPositionLeft.position, _defalutIndex);
    }

    public void DefaultAttackRight()
    {
        CreateBullet(_bulletSpawnPositionRight.position, _bullets[_defalutIndex], _defalutIndex);
        CreateDefaultBulletEffect(_bulletSpawnPositionRight.position, _defalutIndex);
    }

    public void HeavyAttackLeft()
    {
        CreateBullet(_bulletSpawnPositionLeft.position, _bullets[_heavyIndex], _heavyIndex);
        CreateHeavyBulletEffect(_bulletSpawnPositionLeft.position, _heavyIndex);
    }
    public void HeavyAttackRight()
    {
        CreateBullet(_bulletSpawnPositionRight.position, _bullets[_heavyIndex], _heavyIndex);
        CreateHeavyBulletEffect(_bulletSpawnPositionRight.position, _heavyIndex);
    }

    public void LastHeavyAttack()
    {
        Vector3 lastBulletPosition = _bulletSpawnPositionLeft.position - _bulletSpawnPositionRight.position;
        Vector3 spawnPosition = _bulletSpawnPositionRight.position + (lastBulletPosition / 2);
        CreateBullet(spawnPosition, _bullets[_lastHeavyIndex], _lastHeavyIndex);
        Vector3 startEffectPosition = spawnPosition + transform.forward;
        CreateHeavyBulletEffect(startEffectPosition, _lastHeavyIndex);
    }

    private void CreateBullet(Vector3 spawnPosition, HookBullet bulletKind, int index)
    {
        spawnPosition = spawnPosition + transform.forward;
        HookBullet bullet = Instantiate(bulletKind, spawnPosition, Quaternion.identity);

        bullet.gameObject.SetActive(true);
        bullet.transform.forward = transform.forward;
    }
    private void CreateDefaultBulletEffect(Vector3 spawnPosition, int index)
    {
        GameObject effect = Instantiate(_bulletCreateEffect[index], spawnPosition, transform.rotation);
        DefaultBulletEffectRotate(effect);   
    }
    private void CreateHeavyBulletEffect(Vector3 spawnPosition, int index)
    {
        GameObject effect = Instantiate(_bulletCreateEffect[index], spawnPosition, transform.rotation);
        effect.transform.forward = transform.forward;
    }
    private void DefaultBulletEffectRotate(GameObject effect)
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

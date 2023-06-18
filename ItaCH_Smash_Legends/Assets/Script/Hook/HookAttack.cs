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
    private Transform _bulletSpawnPositionLeft;
    private Transform _bulletSpawnPositionRight;
    private GameObject[] _bulletContainers = new GameObject[5];
    private HookBullet[] _bullets = new HookBullet[5];
    private GameObject[] _bulletCreateEffect = new GameObject[3];


    public GameObject Parrot;
    private ParticleSystem _shotEffect;

    private int _defalutIndex = 0;
    private int _heavyIndex = 1;
    private int _lastHeavyIndex = 2;
    private int _skillIndex = 3;
    private int _skillHeavyIndex = 4;

    private float _jumpRotateValue = 45f;
    private Vector3 _jumpUpDashPower = new Vector3(0, 0.3f, 0);
    private Vector3 _jumpDashPower;
    private Vector3 _defaultSkillBulletRatate = new Vector3(0, -5f, 0);
    private Vector3 _heavySkillBulletRatate = new Vector3(0, -7f, 0);

    private float _hookRotateValue;

    private void Start()
    {
        nextTransitionMinValue = 0.6f;
        _bulletSpawnPositionLeft = transform.GetChild(6).GetChild(0).GetChild(2).GetChild(0).transform;
        _bulletSpawnPositionRight = transform.GetChild(6).GetChild(0).GetChild(3).GetChild(0).transform;
        for (int i = 0; i < _bulletContainers.Length; ++i)
        {
            _bulletContainers[i] = transform.GetChild(i).gameObject;
            _bullets[i] = _bulletContainers[i].transform.GetChild(0).GetComponent<HookBullet>();
        }
        for (int i = 0; i < _bulletCreateEffect.Length; ++i)
        {
            _bulletCreateEffect[i] = _bulletContainers[i].transform.GetChild(1).gameObject;
        }

        _shotEffect = Parrot.transform.GetChild(0).GetComponent<ParticleSystem>();
    }
    public override void AttackOnDash()
    {

        if (CurrentPossibleComboCount == COMBO_FINISH_COUNT && playerStatus.CurrentState == PlayerStatus.State.ComboAttack)
        {
            rigidbodyAttack.AddForce(transform.forward * (defaultDashPower * -0.4f), ForceMode.Impulse);

        }
        if (CurrentPossibleComboCount == MAX_POSSIBLE_ATTACK_COUNT &&
            playerStatus.CurrentState == PlayerStatus.State.HeavyAttack)
        {
            rigidbodyAttack.AddForce(transform.forward * (defaultDashPower * -0.65f), ForceMode.Impulse);
        }

        if (playerStatus.IsJump == false)
        {
            _jumpDashPower = transform.forward * -0.15f;
            rigidbodyAttack.velocity = Vector3.zero;
            rigidbodyAttack.AddForce(_jumpUpDashPower + _jumpDashPower, ForceMode.Impulse);
        }

    }
    public override void DefaultAttack()
    {
        if (IsPossibleFirstAttack())
        {
            playerStatus.CurrentState = PlayerStatus.State.ComboAttack;
            animator.Play(AnimationHash.FirstAttack);
        }

        if (isFirstAttack && CurrentPossibleComboCount == COMBO_SECOND_COUNT)
        {
            isSecondAttack = true;
        }
    }
    public override void HeavyAttack()
    {
        base.HeavyAttack();
    }
    public override void JumpAttack()
    {

        if (playerStatus.IsJump == false)
        {
            if (CurrentPossibleComboCount == MAX_POSSIBLE_ATTACK_COUNT)
            {
                animator.SetTrigger(AnimationHash.JumpAttack);
                return;
            }
            else if (CurrentPossibleComboCount == COMBO_SECOND_COUNT)
            {
                animator.SetBool(AnimationHash.JumpSecondAttack, true);
                return;
            }
            else if (CurrentPossibleComboCount == COMBO_FINISH_COUNT)
            {
                animator.SetBool(AnimationHash.JumpFinishAttack, true);
                return;
            }
        }
    }
    public override void SkillAttack()
    {
        if (playerStatus.CurrentState == PlayerStatus.State.Run ||
            playerStatus.CurrentState == PlayerStatus.State.Idle)
        {
            animator.Play(AnimationHash.SkillAttack);
            if (Parrot.activeSelf == true)
            {
                Parrot.SetActive(false);
            }
            Parrot.SetActive(true);
            playerStatus.CurrentState = PlayerStatus.State.SkillAttack;
        }
    }
    public void DefaultAttackLeft()
    {
        CreateBullet(_bulletSpawnPositionLeft.position, _bullets[_defalutIndex]);
        CreateBulletEffect(_bulletSpawnPositionLeft.position, _defalutIndex);

    }
    public void DefaultAttackRight()
    {
        CreateBullet(_bulletSpawnPositionRight.position, _bullets[_defalutIndex]);
        CreateBulletEffect(_bulletSpawnPositionRight.position, _defalutIndex);
    }
    public void HeavyAttackLeft()
    {
        CreateBullet(_bulletSpawnPositionLeft.position, _bullets[_heavyIndex]);
        CreateBulletEffect(_bulletSpawnPositionLeft.position, _heavyIndex);
    }
    public void HeavyAttackRight()
    {
        CreateBullet(_bulletSpawnPositionRight.position, _bullets[_heavyIndex]);
        CreateBulletEffect(_bulletSpawnPositionRight.position, _heavyIndex);
    }
    public void LastHeavyAttack()
    {
        Vector3 lastBulletPosition = _bulletSpawnPositionLeft.position - _bulletSpawnPositionRight.position;
        Vector3 spawnPosition = _bulletSpawnPositionRight.position + (lastBulletPosition / 2);
        CreateBullet(spawnPosition, _bullets[_lastHeavyIndex]);
        Vector3 startEffectPosition = spawnPosition + transform.forward;
        CreateBulletEffect(startEffectPosition, _lastHeavyIndex);
    }
    public void JumpAttackLeft()
    {
        --CurrentPossibleComboCount;
        DefaultAttackLeft();
        AttackOnDash();
    }
    public void JumpAttackRight()
    {
        --CurrentPossibleComboCount;
        DefaultAttackRight();
        AttackOnDash();

    }
    public void SkillAttackBullet()
    {
        if (Parrot.activeSelf)
        {
            CreateSkillBullet();
            _shotEffect.Play();
        }
    }
    private HookBullet CreateBullet(Vector3 spawnPosition, HookBullet bulletKind)
    {
        if (playerStatus.IsJump)
        {
            spawnPosition = spawnPosition + transform.forward;
        }

        HookBullet bullet = Instantiate(bulletKind, spawnPosition, Quaternion.identity);

        if (playerStatus.IsJump)
        {
            bullet.transform.forward = transform.forward;

        }
        else
        {
            bullet.transform.forward = transform.forward;
            bullet.transform.Rotate(JumpBulletRotate(_jumpRotateValue));
        }

        bullet.gameObject.SetActive(true);

        return bullet;
    }
    private void CreateSkillBullet()
    {
        if (playerStatus.CurrentState == PlayerStatus.State.HeavyAttack)
        {
            CreateBullet(Parrot.transform.position, _bullets[_skillHeavyIndex]).transform.Rotate(_defaultSkillBulletRatate);
        }
        else
        {
            CreateBullet(Parrot.transform.position, _bullets[_skillIndex]).transform.Rotate(_heavySkillBulletRatate);
        }
    }
    private void CreateBulletEffect(Vector3 spawnPosition, int index)
    {
        GameObject effect = Instantiate(_bulletCreateEffect[index], spawnPosition, transform.rotation);

        if (playerStatus.CurrentState == PlayerStatus.State.HeavyAttack)
        {
            effect.transform.forward = transform.forward;
        }
        else
        {
            DefaultBulletEffectRotate(effect, _jumpRotateValue);
        }
        effect.SetActive(true);
    }
    private void DefaultBulletEffectRotate(GameObject effect, float value)
    {
        _hookRotateValue = transform.rotation.eulerAngles.y;
        if (_hookRotateValue == 0)
        {
            if (playerStatus.IsJump == true)
            {
                effect.transform.rotation = Quaternion.Euler(-90, 0, 0);
            }
            else
            {
                effect.transform.rotation = Quaternion.Euler(-value, 0, 0);
            }
        }
        else if (_hookRotateValue == 45)
        {
            if (playerStatus.IsJump == true)
            {
                effect.transform.rotation = Quaternion.Euler(-90, 45, 0);
            }
            else
            {
                effect.transform.rotation = Quaternion.Euler(-value, 45, 0);
            }
        }
        else if (_hookRotateValue == 90)
        {
            if (playerStatus.IsJump == true)
            {
                effect.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                effect.transform.rotation = Quaternion.Euler(0, 0, 45);
            }
        }
        else if (_hookRotateValue == 135)
        {

            if (playerStatus.IsJump == true)
            {
                effect.transform.rotation = Quaternion.Euler(90, -45, 0);
            }
            else
            {
                effect.transform.rotation = Quaternion.Euler(value, -45, 0);
            }
        }
        else if (_hookRotateValue == 180)
        {
            if (playerStatus.IsJump == true)
            {
                effect.transform.rotation = Quaternion.Euler(90, 0, 0);
            }
            else
            {
                effect.transform.rotation = Quaternion.Euler(value, 0, 0);
            }
        }
        else if (_hookRotateValue == 225)
        {
            if (playerStatus.IsJump == true)
            {
                effect.transform.rotation = Quaternion.Euler(90, 45, 0);
            }
            else
            {
                effect.transform.rotation = Quaternion.Euler(value, 45, 0);
            }
        }
        else if (_hookRotateValue == 270)
        {
            if (playerStatus.IsJump == true)
            {
                effect.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else
            {
                effect.transform.rotation = Quaternion.Euler(0, 0, -value);
            }
        }
        else if (_hookRotateValue == 315)
        {
            if (playerStatus.IsJump == true)
            {
                effect.transform.rotation = Quaternion.Euler(0, 45, -90);
            }
            else
            {
                effect.transform.rotation = Quaternion.Euler(0, 45, -value);
            }
        }
    }
    private Vector3 JumpBulletRotate(float value)
    {
        if (playerStatus.IsJump == false && CurrentPossibleComboCount == 0)
        {
            value = 30f;
        }
        if (transform.forward.x < 0)
        {
            return transform.forward * -value;
        }
        else if (transform.forward.z != 0)
        {
            return new Vector3(value, 0, 0);
        }
        else
        {
            return transform.forward * value;
        }
    }
}

interface IObjectPool
{

}

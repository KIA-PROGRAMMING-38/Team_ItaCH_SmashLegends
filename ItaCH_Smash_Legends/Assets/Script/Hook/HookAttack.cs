using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TreeEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

public class HookAttack : PlayerAttack
{
    private GameObject[] _bulletContainers = new GameObject[5];

    private ObjectPool<HookBullet>[] _bulletPool = new ObjectPool<HookBullet>[5];
    private HookBullet[] _bullets = new HookBullet[5];
    
    private ObjectPool<FireEffect>[] _bulletCreateEffectPool = new ObjectPool<FireEffect>[3];
    private FireEffect[] _bulletCreateEffects = new FireEffect[3];

    private Transform _bulletSpawnPositionLeft;
    private Transform _bulletSpawnPositionRight;
    private ParticleSystem _shotEffect;

    public GameObject Parrot;

    private int _defalutIndex = 0;
    private int _heavyIndex = 1;
    private int _lastHeavyIndex = 2;
    private int _skillIndex = 3;
    private int _skillHeavyIndex = 4;
    private int _rootIndex = 6;
    private int _boneIndex = 0;
    private int _leftWeaponIndex = 2;
    private int _rightWeaponIndex = 3;
    private int _cylinderIndex = 0;

    private float _jumpRotateValue = 45f;

    private Vector3 _jumpUpDashPower = new Vector3(0, 0.3f, 0);
    private Vector3 _jumpDashPower;
    private Vector3 _defaultSkillBulletRatate = new Vector3(0, -5f, 0);
    private Vector3 _heavySkillBulletRatate = new Vector3(0, -7f, 0);
    private Vector3[] _effectEulerAnglesOnJump = {
        new Vector3(-90, 0, 0),
        new Vector3(-90, 45, 0),
        new Vector3(0, 0, 90),
        new Vector3(90, -45, 0),
        new Vector3(90, 0, 0),
        new Vector3(90, 45, 0),
        new Vector3(0, 0, -90),
        new Vector3(0, 45, -90)
    };
    private Vector3[] _effectEulerAngles = {
        new Vector3(-1, 0, 0),
        new Vector3(-1, 45, 0),
        new Vector3(0, 0, 45),
        new Vector3(1, -45, 0),
        new Vector3(1, 0, 0),
        new Vector3(1, 45, 0),
        new Vector3(0, 0, -1),
        new Vector3(0, 45, -1)
    };
    private void Start()
    {
        CreateObjectPool();
        nextTransitionMinValue = 0.6f;
        _bulletSpawnPositionLeft = transform.GetChild(_rootIndex).GetChild(_boneIndex).GetChild(_leftWeaponIndex).GetChild(_cylinderIndex).transform;
        _bulletSpawnPositionRight = transform.GetChild(_rootIndex).GetChild(_boneIndex).GetChild(_rightWeaponIndex).GetChild(_cylinderIndex).transform;
        for (int i = 0; i < _bulletContainers.Length; ++i)
        {
            _bulletContainers[i] = transform.GetChild(i).gameObject;
            _bullets[i] = _bulletContainers[i].transform.GetChild(0).GetComponent<HookBullet>();
        }
        for (int i = 0; i < _bulletCreateEffects.Length; ++i)
        {
            _bulletCreateEffects[i] = _bulletContainers[i].transform.GetChild(1).GetComponent<FireEffect>();
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
        CreateBullet(_bulletSpawnPositionLeft.position, _defalutIndex);
        CreateBulletEffect(_bulletSpawnPositionLeft.position, _defalutIndex);

    }
    public void DefaultAttackRight()
    {
        CreateBullet(_bulletSpawnPositionRight.position, _defalutIndex);
        CreateBulletEffect(_bulletSpawnPositionRight.position, _defalutIndex);
    }
    public void HeavyAttackLeft()
    {
        CreateBullet(_bulletSpawnPositionLeft.position, _heavyIndex);
        CreateBulletEffect(_bulletSpawnPositionLeft.position, _heavyIndex);
    }
    public void HeavyAttackRight()
    {
        CreateBullet(_bulletSpawnPositionRight.position, _heavyIndex);
        CreateBulletEffect(_bulletSpawnPositionRight.position, _heavyIndex);
    }
    public void LastHeavyAttack()
    {
        Vector3 lastBulletPosition = _bulletSpawnPositionLeft.position - _bulletSpawnPositionRight.position;
        Vector3 spawnPosition = _bulletSpawnPositionRight.position + (lastBulletPosition / 2);
        CreateBullet(spawnPosition, _lastHeavyIndex);
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
    private HookBullet CreateBullet(Vector3 spawnPosition, int bulletIndex)
    {
        if (playerStatus.IsJump)
        {
            spawnPosition = spawnPosition + transform.forward;
        }
        
        HookBullet bullet = _bulletPool[bulletIndex].Get();
        bullet.transform.position = spawnPosition;
        
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
            CreateBullet(Parrot.transform.position, _skillHeavyIndex).transform.Rotate(_defaultSkillBulletRatate);
        }
        else
        {
            CreateBullet(Parrot.transform.position, _skillIndex).transform.Rotate(_heavySkillBulletRatate);
        }
    }
    private void CreateBulletEffect(Vector3 spawnPosition, int index)
    {
        FireEffect effect = _bulletCreateEffectPool[index].Get();
        effect.transform.position = spawnPosition;

        if (playerStatus.CurrentState == PlayerStatus.State.HeavyAttack)
        {
            effect.transform.forward = transform.forward;
        }
        else
        {
            DefaultBulletEffectRotate(effect.gameObject, _jumpRotateValue);
        }
        effect.gameObject.SetActive(true);
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
    private void DefaultBulletEffectRotate(GameObject effect, float value)
    {
        if (IsDiagonalAttack())
        {
            int direction = (int)transform.rotation.eulerAngles.y / 45;
            if (playerStatus.IsJump)
            {
                effect.transform.rotation = Quaternion.Euler(_effectEulerAnglesOnJump[direction]);
            }
            else
            {
                Vector3 eulerAngle = CalculateEffectEulerAngle(direction, value);
                effect.transform.rotation = Quaternion.Euler(eulerAngle);
            }
        }
    }
    private Vector3 CalculateEffectEulerAngle(int direction, float value)
    {
        Vector3 currentEulerAngle = _effectEulerAngles[direction];

        currentEulerAngle.x *= value;
        if (currentEulerAngle.z != 45)
        {
            currentEulerAngle.z *= value;
        }

        return currentEulerAngle;
    }
    private bool IsDiagonalAttack() => transform.rotation.eulerAngles.y % 45 == 0;
    
    private void CreateObjectPool()
    {
        _bulletPool[_defalutIndex] = new ObjectPool<HookBullet>(CreateBulletOnPool, GetPoolBullet, ReturnBulletToPool, (bullet) => Destroy(bullet.gameObject), true, 50, 500);
        _bulletPool[_heavyIndex] = new ObjectPool<HookBullet>(CreateHeavyBulletOnPool, GetPoolBullet, ReturnBulletToPool, (bullet) => Destroy(bullet.gameObject), true, 50, 500);
        _bulletPool[_lastHeavyIndex] = new ObjectPool<HookBullet>(CreateLastHeavyBulletOnPool, GetPoolBullet, ReturnBulletToPool, (bullet) => Destroy(bullet.gameObject), true, 50, 500);
        _bulletPool[_skillIndex] = new ObjectPool<HookBullet>(CreateSkillBulletOnPool, GetPoolBullet, ReturnBulletToPool, (bullet) => Destroy(bullet.gameObject), true, 50, 500);
        _bulletPool[_skillHeavyIndex] = new ObjectPool<HookBullet>(CreateSkillHeavyBulletOnPool, GetPoolBullet, ReturnBulletToPool, (bullet) => Destroy(bullet.gameObject), true, 50, 500);
        
        _bulletCreateEffectPool[_defalutIndex] = new ObjectPool<FireEffect>(CreateDefaultBulletFireEffectOnPool, GetPoolBulletFireEffect, ReturnBulletFireEffect, (effect) => Destroy(effect.gameObject), true, 50, 500);
        _bulletCreateEffectPool[_heavyIndex] = new ObjectPool<FireEffect>(CreateHeavyBulletFireEffectOnPool, GetPoolBulletFireEffect, ReturnBulletFireEffect, (effect) => Destroy(effect.gameObject), true, 50, 500);
        _bulletCreateEffectPool[_lastHeavyIndex] = new ObjectPool<FireEffect>(CreateLastHeavyBulletFireEffectOnPool, GetPoolBulletFireEffect, ReturnBulletFireEffect, (effect) => Destroy(effect.gameObject), true, 50, 500);

    }
    private HookBullet CreateBulletOnPool()
    {
        HookBullet bullet = Instantiate(_bullets[_defalutIndex]);
        bullet.Pool = _bulletPool[_defalutIndex];
        return bullet;
    }
    private HookBullet CreateHeavyBulletOnPool()
    {
        HookBullet bullet = Instantiate(_bullets[_heavyIndex]);
        bullet.Pool = _bulletPool[_heavyIndex];
        return bullet;
    }
    private HookBullet CreateLastHeavyBulletOnPool()
    {
        HookBullet bullet = Instantiate(_bullets[_lastHeavyIndex]);
        bullet.Pool = _bulletPool[_lastHeavyIndex];
        return bullet;
    }
    private HookBullet CreateSkillBulletOnPool()
    {
        HookBullet bullet = Instantiate(_bullets[_skillIndex]);
        bullet.Pool = _bulletPool[_skillIndex];
        return bullet;
    }
    private HookBullet CreateSkillHeavyBulletOnPool()
    {
        HookBullet bullet = Instantiate(_bullets[_skillHeavyIndex]);
        bullet.Pool = _bulletPool[_skillHeavyIndex];
        return bullet;
    }
    private void GetPoolBullet(HookBullet bullet) => bullet.gameObject.SetActive(true);
    private void ReturnBulletToPool(HookBullet bullet) => bullet.gameObject.SetActive(false);
    private FireEffect CreateDefaultBulletFireEffectOnPool()
    {
        FireEffect effect = Instantiate(_bulletCreateEffects[_defalutIndex]);
        effect.pool = _bulletCreateEffectPool[_defalutIndex];
        return effect;
    }
    private FireEffect CreateHeavyBulletFireEffectOnPool()
    {
        FireEffect effect = Instantiate(_bulletCreateEffects[_heavyIndex]);
        effect.pool = _bulletCreateEffectPool[_heavyIndex];
        return effect;
    }
    private FireEffect CreateLastHeavyBulletFireEffectOnPool()
    {
        FireEffect effect = Instantiate(_bulletCreateEffects[_lastHeavyIndex]);
        effect.pool = _bulletCreateEffectPool[_lastHeavyIndex];
        return effect;
    }
    private void GetPoolBulletFireEffect(FireEffect effect) => effect.gameObject.SetActive(true);
    private void ReturnBulletFireEffect(FireEffect effect) => effect.gameObject.SetActive(false);
}

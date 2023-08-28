using UnityEngine;
using UnityEngine.Pool;

public class HookAttack : PlayerAttack
{
    private enum DashType
    {
        Default,
        Heavy,
        Jump
    }
    private enum BulletType
    {
        Default,
        FinishDefault,
        Heavy,
        FinishHeavy,
        Skill,
        HeavySkill
    }
    private enum FirePosition
    {
        Left,
        Right
    }
    private enum FireEffectType
    {
        Default,
        Heavy,
        FinishHeavy
    }

    private GameObject[] _bulletContainers = new GameObject[6];

    private ObjectPool<HookBullet>[] _bulletPool = new ObjectPool<HookBullet>[6];
    private HookBullet[] _bullets = new HookBullet[6];

    private ObjectPool<FireEffect>[] _bulletCreateEffectPool = new ObjectPool<FireEffect>[3];
    private FireEffect[] _bulletCreateEffects = new FireEffect[3];

    private Transform _bulletSpawnPositionLeft;
    private Transform _bulletSpawnPositionRight;
    private ParticleSystem _shotEffect;
    [SerializeField]
    private GameObject _parrot;

    private int _rootIndex = 7;
    private int _boneIndex = 0;
    private int _leftWeaponIndex = 2;
    private int _rightWeaponIndex = 3;
    private int _cylinderIndex = 0;

    private float _jumpRotationValue = 45f;
    private float _defaultDashPower;
    private float _heavyDashPower;
    private float _jumpDashPower;

    private Vector3 _jumpUpDashPower = new Vector3(0, 0.3f, 0);
    private Vector3 _defaultSkillBulletRotation = new Vector3(0, -7f, 0);
    private Vector3 _heavySkillBulletRotation = new Vector3(0, -5f, 0);
    private Vector3 _groundPosition;
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

    private float _correctionDefaultPower = -0.4f;
    private float _correctionHeavyPower = -0.65f;
    private float _correctionJumpPower = -0.15f;

    private void Start()
    {
        _bulletSpawnPositionLeft = transform.GetChild(_rootIndex).GetChild(_boneIndex).GetChild(_leftWeaponIndex).GetChild(_cylinderIndex).transform;
        _bulletSpawnPositionRight = transform.GetChild(_rootIndex).GetChild(_boneIndex).GetChild(_rightWeaponIndex).GetChild(_cylinderIndex).transform;
        for (int i = 0; i < _bulletContainers.Length; ++i)
        {
            _bulletContainers[i] = transform.GetChild(i).gameObject;
            _bullets[i] = _bulletContainers[i].transform.GetChild(0).GetComponent<HookBullet>();
        }
        _bulletCreateEffects[(int)FireEffectType.Default] = _bulletContainers[(int)BulletType.Default].transform.GetChild(1).GetComponent<FireEffect>();
        _bulletCreateEffects[(int)FireEffectType.Heavy] = _bulletContainers[(int)BulletType.Heavy].transform.GetChild(1).GetComponent<FireEffect>();
        _bulletCreateEffects[(int)FireEffectType.FinishHeavy] = _bulletContainers[(int)BulletType.FinishHeavy].transform.GetChild(1).GetComponent<FireEffect>();
        CreateObjectPool();
        _shotEffect = _parrot.transform.GetChild(0).GetComponent<ParticleSystem>();

        Vector3 calibratePosition = new Vector3(0, 0.05f, 0);
        _groundPosition = transform.position + calibratePosition;

        _defaultDashPower = legendController.Stat.DashPower * _correctionDefaultPower;
        _heavyDashPower = legendController.Stat.DashPower * _correctionHeavyPower;
        _jumpDashPower = legendController.Stat.DashPower * _correctionJumpPower;
    }

    private void OnDisable()
    {
        if (_parrot.activeSelf)
        {
            _parrot.SetActive(false);
        }
    }

    private void DashOnAnimationEvent(DashType dashType)
    {
        attackRigidbody.AddForce(GetDashPower(dashType), ForceMode.Impulse);

        Vector3 GetDashPower(DashType dashType)
        {
            attackRigidbody.velocity = Vector3.zero;

            switch (dashType)
            {
                case DashType.Default:
                    return transform.forward * _defaultDashPower;
                case DashType.Heavy:
                    return transform.forward * _heavyDashPower;
                case DashType.Jump:
                    Vector3 jumpDashPower = _jumpDashPower * transform.forward;
                    return _jumpUpDashPower + jumpDashPower;
                default:
                    return Vector3.zero;
            }
        }
    }

    private void SetParrotOnAnimationEvent()
    {
        if (_parrot.activeSelf)
        {
            _parrot.SetActive(false);
        }

        _parrot.SetActive(true);
    }

    private Vector3 GetFirePosition(FirePosition firePosition)
    {
        switch (firePosition)
        {
            case FirePosition.Left:
                return _bulletSpawnPositionLeft.position;
            case FirePosition.Right:
                return _bulletSpawnPositionRight.position;
            default:
                return Vector3.zero;
        }
    }
    private void FireFromLeftHandOnAnimationEvent(BulletType bulletType)
    {
        GetCreateBullet(GetFirePosition(FirePosition.Left), (int)bulletType);
        CreateBulletEffect(GetFirePosition(FirePosition.Left), (int)bulletType);
    }
    private void FireFromRightHandOnAnimationEvent(BulletType bulletType)
    {
        GetCreateBullet(GetFirePosition(FirePosition.Right), (int)bulletType);
        CreateBulletEffect(GetFirePosition(FirePosition.Right), (int)bulletType);
    }

    private void FinishHeavyAttackOnAnimationEvent()
    {
        Vector3 finishBulletPosition = _bulletSpawnPositionLeft.position - _bulletSpawnPositionRight.position;
        Vector3 spawnPosition = _bulletSpawnPositionRight.position + (finishBulletPosition / 2);
        GetCreateBullet(spawnPosition, (int)BulletType.FinishHeavy);
        Vector3 startEffectPosition = spawnPosition + transform.forward;
        CreateBulletEffect(startEffectPosition, (int)BulletType.FinishHeavy);
    }

    private void FireWithSkillOnAnimationEvent(BulletType bulletType)
    {
        if (_parrot.activeSelf)
        {
            CreateSkillBullet(bulletType);
            _shotEffect.Play();
        }
    }
    private HookBullet GetCreateBullet(Vector3 spawnPosition, int bulletIndex)
    {
        if (IsGroundAttack(_groundPosition))
        {
            spawnPosition = spawnPosition + transform.forward;
        }

        HookBullet bullet = _bulletPool[bulletIndex].Get();
        bullet.transform.position = spawnPosition;
        bullet.transform.forward = transform.forward;

        if (IsGroundAttack(_groundPosition) == false)
        {
            bullet.transform.Rotate(JumpBulletRotate(_jumpRotationValue, bulletIndex));
        }

        bullet.gameObject.SetActive(true);
        return bullet;
    }
    private void CreateSkillBullet(BulletType bulletType)
    {
        Vector3 bulletRotation;

        if (bulletType == BulletType.Skill)
        {
            bulletRotation = _defaultSkillBulletRotation;
        }
        else
        {
            bulletRotation = _heavySkillBulletRotation;
        }

        GetCreateBullet(_parrot.transform.position, (int)bulletType).transform.Rotate(bulletRotation);
    }
    private void CreateBulletEffect(Vector3 spawnPosition, int bulletType)
    {
        FireEffect effect = _bulletCreateEffectPool[GetFireEffectIndex(bulletType)].Get();
        effect.transform.position = spawnPosition;

        if (bulletType == (int)BulletType.Heavy || bulletType == (int)BulletType.FinishHeavy)
        {
            effect.transform.forward = transform.forward;
        }
        else
        {
            DefaultBulletEffectRotate(effect.gameObject, _jumpRotationValue);
        }

        effect.gameObject.SetActive(true);

        int GetFireEffectIndex(int bulletType)
        {
            if (bulletType == (int)HookAttack.BulletType.Default)
            {
                return bulletType;
            }
            else
            {
                return bulletType - 1;
            }
        }
    }

    private Vector3 JumpBulletRotate(float value, int bulletType)
    {
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

            if (IsGroundAttack(_groundPosition))
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
    private bool IsGroundAttack(Vector3 position) => transform.position.y < position.y;
    private void CreateObjectPool()
    {
        _bulletPool[(int)BulletType.Default] = new ObjectPool<HookBullet>(CreateBulletOnPool, GetPoolBullet, ReturnBulletToPool, (bullet) => Destroy(bullet.gameObject), true, 50, 500);
        _bulletPool[(int)BulletType.FinishDefault] = new ObjectPool<HookBullet>(CreateFinishComboBulletOnPool, GetPoolBullet, ReturnBulletToPool, (bullet) => Destroy(bullet.gameObject), true, 50, 500);
        _bulletPool[(int)BulletType.Heavy] = new ObjectPool<HookBullet>(CreateHeavyBulletOnPool, GetPoolBullet, ReturnBulletToPool, (bullet) => Destroy(bullet.gameObject), true, 50, 500);
        _bulletPool[(int)BulletType.FinishHeavy] = new ObjectPool<HookBullet>(CreateLastHeavyBulletOnPool, GetPoolBullet, ReturnBulletToPool, (bullet) => Destroy(bullet.gameObject), true, 50, 500);
        _bulletPool[(int)BulletType.Skill] = new ObjectPool<HookBullet>(CreateSkillBulletOnPool, GetPoolBullet, ReturnBulletToPool, (bullet) => Destroy(bullet.gameObject), true, 50, 500);
        _bulletPool[(int)BulletType.HeavySkill] = new ObjectPool<HookBullet>(CreateSkillHeavyBulletOnPool, GetPoolBullet, ReturnBulletToPool, (bullet) => Destroy(bullet.gameObject), true, 50, 500);

        _bulletCreateEffectPool[(int)FireEffectType.Default] = new ObjectPool<FireEffect>(CreateDefaultBulletFireEffectOnPool, GetPoolBulletFireEffect, ReturnBulletFireEffect, (effect) => Destroy(effect.gameObject), true, 50, 500);
        _bulletCreateEffectPool[(int)FireEffectType.Heavy] = new ObjectPool<FireEffect>(CreateHeavyBulletFireEffectOnPool, GetPoolBulletFireEffect, ReturnBulletFireEffect, (effect) => Destroy(effect.gameObject), true, 50, 500);
        _bulletCreateEffectPool[(int)FireEffectType.FinishHeavy] = new ObjectPool<FireEffect>(CreateLastHeavyBulletFireEffectOnPool, GetPoolBulletFireEffect, ReturnBulletFireEffect, (effect) => Destroy(effect.gameObject), true, 50, 500);
    }
    private HookBullet CreateBulletOnPool()
    {
        HookBullet bullet = Instantiate(_bullets[(int)BulletType.Default]);
        bullet.Pool = _bulletPool[(int)BulletType.Default];
        bullet.constructor = gameObject;
        return bullet;
    }
    private HookBullet CreateFinishComboBulletOnPool()
    {
        HookBullet bullet = Instantiate(_bullets[(int)BulletType.FinishDefault]);
        bullet.Pool = _bulletPool[(int)BulletType.FinishDefault];
        bullet.constructor = gameObject;
        return bullet;
    }
    private HookBullet CreateHeavyBulletOnPool()
    {
        HookBullet bullet = Instantiate(_bullets[(int)BulletType.Heavy]);
        bullet.Pool = _bulletPool[(int)BulletType.Heavy];
        bullet.constructor = gameObject;
        return bullet;
    }
    private HookBullet CreateLastHeavyBulletOnPool()
    {
        HookBullet bullet = Instantiate(_bullets[(int)BulletType.FinishHeavy]);
        bullet.Pool = _bulletPool[(int)BulletType.FinishHeavy];
        bullet.constructor = gameObject;
        return bullet;
    }
    private HookBullet CreateSkillBulletOnPool()
    {
        HookBullet bullet = Instantiate(_bullets[(int)BulletType.Skill]);
        bullet.Pool = _bulletPool[(int)BulletType.Skill];
        bullet.constructor = gameObject;
        return bullet;
    }
    private HookBullet CreateSkillHeavyBulletOnPool()
    {
        HookBullet bullet = Instantiate(_bullets[(int)BulletType.HeavySkill]);
        bullet.Pool = _bulletPool[(int)BulletType.HeavySkill];
        bullet.constructor = gameObject;
        return bullet;
    }
    private void GetPoolBullet(HookBullet bullet) => bullet.gameObject.SetActive(true);
    private void ReturnBulletToPool(HookBullet bullet) => bullet.gameObject.SetActive(false);
    private FireEffect CreateDefaultBulletFireEffectOnPool()
    {
        FireEffect effect = Instantiate(_bulletCreateEffects[(int)FireEffectType.Default]);
        effect.pool = _bulletCreateEffectPool[(int)FireEffectType.Default];
        return effect;
    }
    private FireEffect CreateHeavyBulletFireEffectOnPool()
    {
        FireEffect effect = Instantiate(_bulletCreateEffects[(int)FireEffectType.Heavy]);
        effect.pool = _bulletCreateEffectPool[(int)FireEffectType.Heavy];
        return effect;
    }
    private FireEffect CreateLastHeavyBulletFireEffectOnPool()
    {
        FireEffect effect = Instantiate(_bulletCreateEffects[(int)FireEffectType.FinishHeavy]);
        effect.pool = _bulletCreateEffectPool[(int)FireEffectType.FinishHeavy];
        return effect;
    }
    private void GetPoolBulletFireEffect(FireEffect effect) => effect.gameObject.SetActive(true);
    private void ReturnBulletFireEffect(FireEffect effect) => effect.gameObject.SetActive(false);
}
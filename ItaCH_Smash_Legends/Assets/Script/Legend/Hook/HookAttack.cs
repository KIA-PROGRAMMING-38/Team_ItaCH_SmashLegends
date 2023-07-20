using UnityEngine;
using UnityEngine.Pool;

public class HookAttack : PlayerAttack
{
    private GameObject[] _bulletContainers = new GameObject[6];

    private ObjectPool<HookBullet>[] _bulletPool = new ObjectPool<HookBullet>[6];
    private HookBullet[] _bullets = new HookBullet[6];

    private ObjectPool<FireEffect>[] _bulletCreateEffectPool = new ObjectPool<FireEffect>[3];
    private FireEffect[] _bulletCreateEffects = new FireEffect[3];

    private Transform _bulletSpawnPositionLeft;
    private Transform _bulletSpawnPositionRight;
    private ParticleSystem _shotEffect;

    public GameObject Parrot;

    private int _defaultIndex = 0;
    private int _finishComboIndex = 1;
    private int _heavyIndex = 2;
    private int _lastHeavyIndex = 3;
    private int _skillIndex = 4;
    private int _skillHeavyIndex = 5;
    private int _rootIndex = 7;
    private int _boneIndex = 0;
    private int _leftWeaponIndex = 2;
    private int _rightWeaponIndex = 3;
    private int _cylinderIndex = 0;

    private int _defaultFireEffect = 0;
    private int _heavyFireEffect = 1;
    private int _lastHeavyFireEffect = 2;

    private float _jumpRotateValue = 45f;
    private float _defaultDashPower;
    private float _heavyDashPower;

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
        _bulletSpawnPositionLeft = transform.GetChild(_rootIndex).GetChild(_boneIndex).GetChild(_leftWeaponIndex).GetChild(_cylinderIndex).transform;
        _bulletSpawnPositionRight = transform.GetChild(_rootIndex).GetChild(_boneIndex).GetChild(_rightWeaponIndex).GetChild(_cylinderIndex).transform;
        for (int i = 0; i < _bulletContainers.Length; ++i)
        {
            _bulletContainers[i] = transform.GetChild(i).gameObject;
            _bullets[i] = _bulletContainers[i].transform.GetChild(0).GetComponent<HookBullet>();
        }
        _bulletCreateEffects[_defaultFireEffect] = _bulletContainers[_defaultIndex].transform.GetChild(1).GetComponent<FireEffect>();
        _bulletCreateEffects[_heavyFireEffect] = _bulletContainers[_heavyIndex].transform.GetChild(1).GetComponent<FireEffect>();
        _bulletCreateEffects[_lastHeavyFireEffect] = _bulletContainers[_lastHeavyIndex].transform.GetChild(1).GetComponent<FireEffect>();

        CreateObjectPool();
        _shotEffect = Parrot.transform.GetChild(0).GetComponent<ParticleSystem>();

        _defaultDashPower = legendController.Stat.DashPower * -0.4f;
        _heavyDashPower = legendController.Stat.DashPower * -0.65f;
    }
    private void OnDisable()
    {
        if (Parrot.activeSelf)
        {
            Parrot.SetActive(false);
        }
    }
    public override void DashOnAnimationEvent()
    {
        //if (CurrentPossibleComboCount == COMBO_FINISH_COUNT && playerStatus.CurrentState == PlayerStatus.State.ComboAttack)
        //{
        //    rigidbodyAttack.AddForce(transform.forward * (_defaultDashPower), ForceMode.Impulse);

        //}
        //if (CurrentPossibleComboCount == MAX_POSSIBLE_ATTACK_COUNT &&
        //    playerStatus.CurrentState == PlayerStatus.State.HeavyAttack)
        //{
        //    rigidbodyAttack.AddForce(transform.forward * (_heavyDashPower ), ForceMode.Impulse);
        //}

        //if (playerStatus.IsJump == false)
        //{
        //    _jumpDashPower = transform.forward * -0.15f;
        //    rigidbodyAttack.velocity = Vector3.zero;
        //    rigidbodyAttack.AddForce(_jumpUpDashPower + _jumpDashPower, ForceMode.Impulse);
        //}

    }

    //public override void SkillAttack()
    //{
    //    if (playerStatus.CurrentState == PlayerStatus.State.Run ||
    //        playerStatus.CurrentState == PlayerStatus.State.Idle)
    //    {
    //        animator.Play(AnimationHash.SkillAttack);
    //        if (Parrot.activeSelf == true)
    //        {
    //            Parrot.SetActive(false);
    //        }
    //        Parrot.SetActive(true);
    //        playerStatus.CurrentState = PlayerStatus.State.SkillAttack;
    //    }
    //}
    public void DefaultAttackLeft()
    {
        CreateBullet(_bulletSpawnPositionLeft.position, _defaultIndex);
        CreateBulletEffect(_bulletSpawnPositionLeft.position, _defaultFireEffect);
    }
    public void DefaultAttackRight()
    {
        // TODO : 함수 분할
        //CreateBullet(_bulletSpawnPositionRight.position, _finishComboIndex);
        CreateBullet(_bulletSpawnPositionRight.position, _defaultIndex);
        CreateBulletEffect(_bulletSpawnPositionRight.position, _defaultFireEffect);
    }
    public void HeavyAttackLeft()
    {
        CreateBullet(_bulletSpawnPositionLeft.position, _heavyIndex);
        CreateBulletEffect(_bulletSpawnPositionLeft.position, _heavyFireEffect);
    }
    public void HeavyAttackRight()
    {
        CreateBullet(_bulletSpawnPositionRight.position, _heavyIndex);
        CreateBulletEffect(_bulletSpawnPositionRight.position, _heavyFireEffect);
    }
    public void LastHeavyAttack()
    {
        Vector3 lastBulletPosition = _bulletSpawnPositionLeft.position - _bulletSpawnPositionRight.position;
        Vector3 spawnPosition = _bulletSpawnPositionRight.position + (lastBulletPosition / 2);
        CreateBullet(spawnPosition, _lastHeavyIndex);
        Vector3 startEffectPosition = spawnPosition + transform.forward;
        CreateBulletEffect(startEffectPosition, _lastHeavyFireEffect);
    }
    public void JumpAttackLeft()
    {
        DefaultAttackLeft();
        DashOnAnimationEvent();
    }
    public void JumpAttackRight()
    {
        //TODO : 함수 분할
        CreateBullet(_bulletSpawnPositionRight.position, _finishComboIndex);

        CreateBullet(_bulletSpawnPositionRight.position, _defaultIndex);
        CreateBulletEffect(_bulletSpawnPositionRight.position, _defaultFireEffect);
        DashOnAnimationEvent();
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
        // TODO : 점프상태 정의 필요
        //if (playerStatus.IsJump)
        //{
        //    spawnPosition = spawnPosition + transform.forward;
        //}

        HookBullet bullet = _bulletPool[bulletIndex].Get();
        bullet.transform.position = spawnPosition;

        //if (playerStatus.IsJump)
        //{
        //    bullet.transform.forward = transform.forward;
        //    bullet.transform.GetChild(0).transform.forward = bullet.transform.forward;

        //}
        //else
        //{
        bullet.transform.forward = transform.forward;
        bullet.transform.GetChild(0).transform.forward = bullet.transform.forward;
        bullet.transform.Rotate(JumpBulletRotate(_jumpRotateValue));
        //}

        bullet.gameObject.SetActive(true);
        return bullet;
    }
    private void CreateSkillBullet()
    {
        // TODO : 스킬공격 추가타 => 기본공격, 헤비공격 구분 필요
        //if (playerStatus.CurrentState == PlayerStatus.State.HeavyAttack)
        //{
        CreateBullet(Parrot.transform.position, _skillHeavyIndex).transform.Rotate(_defaultSkillBulletRatate);
        //}
        //else
        //{
        //    CreateBullet(Parrot.transform.position, _skillIndex).transform.Rotate(_heavySkillBulletRatate);
        //}
    }
    private void CreateBulletEffect(Vector3 spawnPosition, int index)
    {
        FireEffect effect = _bulletCreateEffectPool[index].Get();
        effect.transform.position = spawnPosition;

        // TODO : 함수 분할 
        //if (playerStatus.CurrentState == PlayerStatus.State.HeavyAttack)
        //{
        //    effect.transform.forward = transform.forward;
        //}
        //else
        //{
        DefaultBulletEffectRotate(effect.gameObject, _jumpRotateValue);
        //}
        effect.gameObject.SetActive(true);
    }
    private Vector3 JumpBulletRotate(float value)
    {
        // TODO : 함수 분할 

        //if (playerStatus.IsJump == false && CurrentPossibleComboCount == 0)
        //{
        //    value = 30f;
        //}
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
            // TODO : 점프 상태 정의 필요
            //if (playerStatus.IsJump)
            //{
            effect.transform.rotation = Quaternion.Euler(_effectEulerAnglesOnJump[direction]);
            //}
            //else
            //{
            Vector3 eulerAngle = CalculateEffectEulerAngle(direction, value);
            effect.transform.rotation = Quaternion.Euler(eulerAngle);
            //}
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
        _bulletPool[_defaultIndex] = new ObjectPool<HookBullet>(CreateBulletOnPool, GetPoolBullet, ReturnBulletToPool, (bullet) => Destroy(bullet.gameObject), true, 50, 500);
        _bulletPool[_finishComboIndex] = new ObjectPool<HookBullet>(CreateFinishComboBulletOnPool, GetPoolBullet, ReturnBulletToPool, (bullet) => Destroy(bullet.gameObject), true, 50, 500);
        _bulletPool[_heavyIndex] = new ObjectPool<HookBullet>(CreateHeavyBulletOnPool, GetPoolBullet, ReturnBulletToPool, (bullet) => Destroy(bullet.gameObject), true, 50, 500);
        _bulletPool[_lastHeavyIndex] = new ObjectPool<HookBullet>(CreateLastHeavyBulletOnPool, GetPoolBullet, ReturnBulletToPool, (bullet) => Destroy(bullet.gameObject), true, 50, 500);
        _bulletPool[_skillIndex] = new ObjectPool<HookBullet>(CreateSkillBulletOnPool, GetPoolBullet, ReturnBulletToPool, (bullet) => Destroy(bullet.gameObject), true, 50, 500);
        _bulletPool[_skillHeavyIndex] = new ObjectPool<HookBullet>(CreateSkillHeavyBulletOnPool, GetPoolBullet, ReturnBulletToPool, (bullet) => Destroy(bullet.gameObject), true, 50, 500);

        _bulletCreateEffectPool[_defaultFireEffect] = new ObjectPool<FireEffect>(CreateDefaultBulletFireEffectOnPool, GetPoolBulletFireEffect, ReturnBulletFireEffect, (effect) => Destroy(effect.gameObject), true, 50, 500);
        _bulletCreateEffectPool[_heavyFireEffect] = new ObjectPool<FireEffect>(CreateHeavyBulletFireEffectOnPool, GetPoolBulletFireEffect, ReturnBulletFireEffect, (effect) => Destroy(effect.gameObject), true, 50, 500);
        _bulletCreateEffectPool[_lastHeavyFireEffect] = new ObjectPool<FireEffect>(CreateLastHeavyBulletFireEffectOnPool, GetPoolBulletFireEffect, ReturnBulletFireEffect, (effect) => Destroy(effect.gameObject), true, 50, 500);
    }
    private HookBullet CreateBulletOnPool()
    {
        HookBullet bullet = Instantiate(_bullets[_defaultIndex]);
        bullet.Pool = _bulletPool[_defaultIndex];
        bullet.constructor = gameObject;
        return bullet;
    }
    private HookBullet CreateFinishComboBulletOnPool()
    {
        HookBullet bullet = Instantiate(_bullets[_finishComboIndex]);
        bullet.Pool = _bulletPool[_finishComboIndex];
        bullet.constructor = gameObject;
        return bullet;
    }
    private HookBullet CreateHeavyBulletOnPool()
    {
        HookBullet bullet = Instantiate(_bullets[_heavyIndex]);
        bullet.Pool = _bulletPool[_heavyIndex];
        bullet.constructor = gameObject;
        return bullet;
    }
    private HookBullet CreateLastHeavyBulletOnPool()
    {
        HookBullet bullet = Instantiate(_bullets[_lastHeavyIndex]);
        bullet.Pool = _bulletPool[_lastHeavyIndex];
        bullet.constructor = gameObject;
        return bullet;
    }
    private HookBullet CreateSkillBulletOnPool()
    {
        HookBullet bullet = Instantiate(_bullets[_skillIndex]);
        bullet.Pool = _bulletPool[_skillIndex];
        bullet.constructor = gameObject;
        return bullet;
    }
    private HookBullet CreateSkillHeavyBulletOnPool()
    {
        HookBullet bullet = Instantiate(_bullets[_skillHeavyIndex]);
        bullet.Pool = _bulletPool[_skillHeavyIndex];
        bullet.constructor = gameObject;
        return bullet;
    }
    private void GetPoolBullet(HookBullet bullet) => bullet.gameObject.SetActive(true);
    private void ReturnBulletToPool(HookBullet bullet) => bullet.gameObject.SetActive(false);
    private FireEffect CreateDefaultBulletFireEffectOnPool()
    {
        FireEffect effect = Instantiate(_bulletCreateEffects[_defaultFireEffect]);
        effect.pool = _bulletCreateEffectPool[_defaultFireEffect];
        return effect;
    }
    private FireEffect CreateHeavyBulletFireEffectOnPool()
    {
        FireEffect effect = Instantiate(_bulletCreateEffects[_heavyFireEffect]);
        effect.pool = _bulletCreateEffectPool[_heavyFireEffect];
        return effect;
    }
    private FireEffect CreateLastHeavyBulletFireEffectOnPool()
    {
        FireEffect effect = Instantiate(_bulletCreateEffects[_lastHeavyFireEffect]);
        effect.pool = _bulletCreateEffectPool[_lastHeavyFireEffect];
        return effect;
    }
    private void GetPoolBulletFireEffect(FireEffect effect) => effect.gameObject.SetActive(true);
    private void ReturnBulletFireEffect(FireEffect effect) => effect.gameObject.SetActive(false);
}
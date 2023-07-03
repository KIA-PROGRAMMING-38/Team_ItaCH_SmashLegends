using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Util.Enum;

public class CharacterStatus : CharacterDefaultStatus
{
    // 게임 중 바뀔 수 있는 스탯들
    public int HealthPoint { get => _currentHealthPoint; set => _currentHealthPoint = value; }
    public int MaxHealthPoint { get => _maxHealthPoint; private set => _maxHealthPoint = value; }
    public int HealthPointRatio { get => _currentHealthPointRatio; set => _currentHealthPointRatio = value; }
    public int PlayerID { get => _playerID; set => _playerID = value; }
    public int Size { get => _size; set => _size = value; }
    public float RespawnTime { get => _currentRespawnTime; set => _currentRespawnTime = value; }
    public TeamType TeamType { get => _teamType; set => _teamType = value; }
    public Vector3 TeamSpawnPoint { get => _spawnPoint; set => _spawnPoint = value; }


    public int DefaultAttackDamage { get => _defaultAttackDamage; set => _defaultAttackDamage = value; }
    public int HeavyAttackDamage { get => _heavyAttackDamage; set => _heavyAttackDamage = value; }
    public int JumpAttackDamage { get => _jumpAttackDamage; set => _jumpAttackDamage = value; }
    public int SkillAttackDamage { get => _skillAttackDamage; set => _skillAttackDamage = value; }
    public int SkillGauage { get => _skillGauage; set => _skillGauage = value; }
    public int SkillGauageRecovery { get => _skillRecovery; set => _skillRecovery = value; }

    public float DefaultKnockbackPower { get => _defaultKnockbackPower; set => _defaultKnockbackPower = value; }
    public float HeavyKnockbackPower { get => _heavyKnockbackPower; set => _heavyKnockbackPower = value; }
    public float HeavyCooltime { get => _heavyCooltime; set => _heavyCooltime = value; }
    public float DashPower { get => _dashpower; set => _dashpower = value; }

    public float JumpAcceleration { get => _jumpAcceleration; set => _jumpAcceleration = value; }
    public float MaxFallingSpeed { get => _maxFallingSpeed; set => _maxFallingSpeed = value; }
    public float GravitationalAcceleration { get => _gravitationalAcceleration; set => _gravitationalAcceleration = value; }
    public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }

    private int _currentHealthPoint;
    private int _maxHealthPoint;
    private int _currentHealthPointRatio;
    private float _currentRespawnTime;
    private const int DEAD_TRIGGER_HP = 0;
    internal bool _isDead = false;

    public event Action<int, int> OnPlayerHealthPointChange;
    public event Action<int> OnPlayerGetDamage;
    public event Action<CharacterStatus> OnPlayerDie;
    public event Action<CharacterStatus> OnPlayerRespawn;
    public event Action<GameObject, int> OnRespawnSetting;
    public event Action OnPlayerDieEffect;
    public event Action OnPlayerDieSmokeEffect;

    private int _playerID;
    private TeamType _teamType;

    private Vector3 _spawnPoint;
    private int _size;

    private int _defaultAttackDamage;
    private int _heavyAttackDamage;
    private int _jumpAttackDamage;
    private int _skillAttackDamage;
    private int _skillGauage;
    private int _skillRecovery;

    private float _defaultKnockbackPower;
    private float _heavyKnockbackPower;
    private float _dashpower;
    private float _heavyCooltime;

    private float _jumpAcceleration;
    private float _maxFallingSpeed;
    private float _gravitationalAcceleration;
    private float _moveSpeed;

    private CharacterType _characterType;
    public CharacterType CharacterType { get => _characterType; set => _characterType = value; }

    private string _name;
    public string Name { get => _name; set => _name = value; }

    private void OnDisable()
    {
        if (this._isDead)
        {
            OnPlayerDieEffect.Invoke();
            RespawnAsync(_currentRespawnTime).Forget();
        }
    }
    public void InitCharacterDefaultData(CharacterType characterType)
    {
        int character = (int)characterType;
        GetCharacterDefaultData(character);
    }
    public void InitStatus()
    {
        InitHP();
        InitSize();
        InitAttackRelevant();
        InitMoveRelevant();
    }
    private void InitHP()
    {
        _currentHealthPoint = base.HpData;
        _maxHealthPoint = base.HpData;
        _currentHealthPointRatio = 100;
        OnPlayerHealthPointChange?.Invoke(_currentHealthPoint, _currentHealthPointRatio);
    }
    private void InitAttackRelevant()
    {
        _defaultAttackDamage = base.DefaultAttackDamageData;
        _heavyAttackDamage = base.HeavyAttackDamageData;
        _jumpAttackDamage = base.JumpAttackDamageData;
        _skillAttackDamage = base.SkillAttackDamageData;
        _defaultKnockbackPower = base.DefaultKnockbackPowerData;
        _heavyKnockbackPower = base.HeavyKnockbackPowerData;
        _dashpower = base.DashPowerData;
        _heavyCooltime = base.HeavyCooltimeData;
        _skillGauage = base.SkillGaugeData;
        _skillRecovery = base.SkillRecoveryData;
    }
    private void InitMoveRelevant()
    {
        _jumpAcceleration = base.JumpAccelerationData;
        _maxFallingSpeed = base.MaxFallingSpeedData;
        _gravitationalAcceleration = base.GravitationalAccelerationData;
        _moveSpeed = base.MoveSpeedData;
    }
    private void InitSize()
    {
        _size = base.SizeData;
    }
    public void GetDamage(int damage) // 피격 판정 시 호출
    {
        int damagedHealthPoint = _currentHealthPoint - damage;
        _currentHealthPoint = Mathf.Max(damagedHealthPoint, DEAD_TRIGGER_HP);
        _currentHealthPointRatio = (_currentHealthPoint * 100) / MaxHealthPoint;
        OnPlayerHealthPointChange.Invoke(_currentHealthPoint, _currentHealthPointRatio);
        OnPlayerGetDamage?.Invoke(damage);
        if (_currentHealthPoint <= DEAD_TRIGGER_HP && !this._isDead)
        {
            OnPlayerDie.Invoke(this);
            OnPlayerDieSmokeEffect.Invoke();
        }
    }
    private async UniTaskVoid RespawnAsync(float respawnTime)
    {
        await UniTask.Delay((int)respawnTime * 1000);
        Respawn();
    }
    public void Respawn()
    {
        this.transform.position = _spawnPoint;
        this.gameObject.SetActive(true);
        this.GetComponent<Collider>().isTrigger = false;
        this._isDead = false;
        OnPlayerRespawn.Invoke(this);
        OnRespawnSetting.Invoke(this.gameObject, _playerID);
        InitHP();
    }
}

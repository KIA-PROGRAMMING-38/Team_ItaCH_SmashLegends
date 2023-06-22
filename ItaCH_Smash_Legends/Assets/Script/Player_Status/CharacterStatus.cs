using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;
using Util.Enum;

public class CharacterStatus : CharacterDefaultStatus
{
    // 게임 중 바뀔 수 있는 스탯들
    public int HealthPoint { get => _currentHealthPoint; set => _currentHealthPoint = value; }
    public int HealthPointRatio { get => _currentHealthPointRatio; set => _currentHealthPointRatio = value; }
    public int PlayerID { get => _playerID; set => _playerID = value; }

    public float RespawnTime { get => _currentRespawnTime; set => _currentRespawnTime = value; }
    public TeamType TeamType { get => _teamType; set => _teamType = value; }
    public Vector3 TeamSpawnPoint { get => _spawnPoint; set => _spawnPoint = value; }

    public int DefaultAttackDamage { get => _defaultAttackDamage; private set => _defaultAttackDamage = value; }
    public int HeavyAttackDamage { get => _heavyAttackDamage; set => _heavyAttackDamage = value; }
    public int JumpAttackDamage { get => _jumpAttackDamage; set => _jumpAttackDamage = value; }
    public int SkillAttackDamage { get => _skillAttackDamage; set => _skillAttackDamage = value; }

    private int _currentHealthPoint;
    private int _currentHealthPointRatio;
    private float _currentRespawnTime;
    private const int DEAD_TRIGGER_HP = 0;
    private bool _isDead = false;

    public event Action<int, int> OnPlayerHealthPointChange;
    public event Action<CharacterStatus> OnPlayerDie;
    public event Action<CharacterStatus> OnPlayerRespawn;

    private int _playerID;
    private TeamType _teamType;

    private Vector3 _spawnPoint;

    private int _defaultAttackDamage;
    private int _heavyAttackDamage;
    private int _jumpAttackDamage;
    private int _skillAttackDamage;
    private CharacterType _characterType;
    public CharacterType CharacterType { get => _characterType; }

    private void Awake()
    {
        InitHP();
        InitAttackDamage();
    }
    public void InitCharacterType(CharacterType characterType)
    {
        _characterType = characterType;
    }
    public void InitHP()
    {
        _currentHealthPoint = base.MaxHealthPoint;
        _currentHealthPointRatio = 100;
        OnPlayerHealthPointChange?.Invoke(_currentHealthPoint, _currentHealthPointRatio);
    }
    public void InitAttackDamage() // 데이터 테이블 구성 이후 반영 필요
    {
        _defaultAttackDamage = 100;
        _heavyAttackDamage = 200;
        _jumpAttackDamage = 100;
        _skillAttackDamage = 200;
    }
    public void GetDamage(int damage) // 피격 판정 시 호출
    {
        int damagedHealthPoint = _currentHealthPoint - damage;
        _currentHealthPoint = Mathf.Max(damagedHealthPoint, DEAD_TRIGGER_HP);
        _currentHealthPointRatio = (_currentHealthPoint * 100) / base.MaxHealthPoint;
        OnPlayerHealthPointChange.Invoke(_currentHealthPoint, _currentHealthPointRatio);
        if (_currentHealthPoint <= DEAD_TRIGGER_HP && !this._isDead)
        {
            this._isDead = true;
            this.gameObject.SetActive(false);
            OnPlayerDie.Invoke(this);            
            RespawnAsync(_currentRespawnTime).Forget();
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
        InitHP();
    }
}

using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Util.Enum;

public class CharacterStatus : MonoBehaviour
{
    private LegendStatData Stat;

    public int HealthPoint { get => _currentHealthPoint; set => _currentHealthPoint = value; }
    public int MaxHP { get => Stat.HP; }
    public int CurrentHPRatio { get => (_currentHealthPoint * 100) / _maxHealthPoint; }

    public int DefaultAttackDamage { get => Stat.DefaultAttackDamage; }    
    public int HeavyAttackDamage { get => Stat.HeavyAttackDamage;}
    public int JumpAttackDamage { get => Stat.JumpAttackDamage; }
    public int SkillAttackDamage { get => Stat.SkillAttackDamage; }
    public int SkillGauge { get => Stat.SkillGauge; }
    public int SkillGauageRecovery { get => Stat.SkillRecovery; }

    public float DefaultKnockbackPower { get => Stat.DefaultKnockbackPower; }
    public float HeavyKnockbackPower { get => Stat.HeavyKnockbackPower; }
    public float HeavyCooltime { get => Stat.HeavyCooltime; }
    public float DashPower { get => Stat.DashPower; }

    public float JumpAcceleration { get => Stat.JumpAcceleration; }
    public float MaxFallingSpeed { get => Stat.MaxFallingSpeed; }
    public float GravitationalAcceleration { get => Stat.GravitationalAcceleration; }
    public float MoveSpeed { get => Stat.MoveSpeed; }

    private int _currentHealthPoint;
    private int _maxHealthPoint;
    
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

    public int PlayerID { get; set; } // UI 로직 수정 이후 없어질 프로퍼티
    public string Name { get; set; } // UI 로직 수정 이후 없어질 프로퍼티
    public TeamType TeamType { get; set; }
    public Vector3 SpawnPoint { get; set; }
    public float RespawnTime { get; set; }
    public CharacterType Character { get; set; }

    private void OnDisable()
    {
        if (this._isDead)
        {
            OnPlayerDieEffect.Invoke();
            OnPlayerDie.Invoke(this);
            RespawnAsync(_currentRespawnTime).Forget();
        }
    }
    
    public void Init(UserData userData)
    {
        PlayerID = userData.Id;
        Name = userData.Name;
        Character = userData.SelectedCharacter;
        Stat = Managers.DataManager.LegendStats[(int)Character];
        _maxHealthPoint = Stat.HP;        
        SetDefaultHP();
    }

    public void SetDefaultHP()
    {
        _currentHealthPoint = _maxHealthPoint * 10;        
        OnPlayerHealthPointChange?.Invoke(_currentHealthPoint, CurrentHPRatio);
        _currentHealthPoint = _maxHealthPoint;
    }

    public void GetDamage(int damage) // 피격 판정 시 호출
    {
        int damagedHealthPoint = _currentHealthPoint - damage;
        _currentHealthPoint = Mathf.Max(damagedHealthPoint, DEAD_TRIGGER_HP);        
        OnPlayerHealthPointChange.Invoke(_currentHealthPoint, CurrentHPRatio);
        OnPlayerGetDamage?.Invoke(damage);
        if (_currentHealthPoint <= DEAD_TRIGGER_HP && !this._isDead)
        {
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
        this.transform.position = SpawnPoint;
        this.gameObject.SetActive(true);
        this.GetComponent<Collider>().isTrigger = false;
        this._isDead = false;
        OnPlayerRespawn.Invoke(this);
        OnRespawnSetting.Invoke(this.gameObject, PlayerID);
        SetDefaultHP();
    }
}

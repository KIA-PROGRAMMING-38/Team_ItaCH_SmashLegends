using System;
using UnityEngine;
public class CharacterStatus : CharacterDefaultStatus
{
    // 게임 중 바뀔 수 있는 스탯들
    public int HealthPoint { get => _currentHealthPoint; set => _currentHealthPoint = value; }
    public int HealthPointRatio { get => _currentHealthPointRatio; set => _currentHealthPointRatio = value; }
    public int PlayerID { get => _playerID; set => _playerID = value; }
    public TeamType TeamType { get => _teamType; set => _teamType = value; }
    public Vector3 TeamSpawnPoint { get => _spawnPoint; set => _spawnPoint = value; }

    private int _currentHealthPoint;
    private int _currentHealthPointRatio;

    private const int DEAD_TRIGGER_HP = 0;

    public event Action<int, int> OnPlayerHealthPointChange;
    public event Action<CharacterStatus> OnPlayerDie;

    private int _playerID;
    private TeamType _teamType;

    private Vector3 _spawnPoint;

    private void Awake()
    {
        InitHP();
    }
    public void InitHP()
    {
        _currentHealthPoint = base.MaxHealthPoint;
        _currentHealthPointRatio = 100;
        OnPlayerHealthPointChange?.Invoke(_currentHealthPoint, _currentHealthPointRatio);
    }
    public void GetDamage(int damage) // 피격 판정 시 호출
    {
        int damagedHealthPoint = _currentHealthPoint - damage;
        _currentHealthPoint = Mathf.Max(damagedHealthPoint, DEAD_TRIGGER_HP);
        _currentHealthPointRatio = (_currentHealthPoint * 100) / base.MaxHealthPoint;
        OnPlayerHealthPointChange.Invoke(_currentHealthPoint, _currentHealthPointRatio);
        if (_currentHealthPoint == DEAD_TRIGGER_HP)
        {
            this.gameObject.SetActive(false);
            OnPlayerDie.Invoke(this);
            Respawn();
        }
    }
    public void Respawn()
    {
        this.transform.position = _spawnPoint;
        this.gameObject.SetActive(true);
        this.GetComponent<Collider>().isTrigger = false;
        InitHP();
    }
}

using System;
using UnityEngine;
public class CharacterStatus : CharacterDefaultStatus
{
    // 게임 중 바뀔 수 있는 스탯들
    public int HealthPoint { get => _currentHealthPoint; set => _currentHealthPoint = value; }
    public int HealthPointRatio { get => _currentHealthPointRatio; set => _currentHealthPointRatio = value; }
    public TeamType TeamType { get => _teamType; set => _teamType = value; }

    private int _currentHealthPoint;
    private int _currentHealthPointRatio;
    
    private const int DEAD_TRIGGER_HP = 0;


    public event Action<int, int> OnPlayerHealthPointChange;
    public event Action OnPlayerDie;

    private TeamType _teamType;
    float time= 0;

    private void Awake()
    {
        InitHP();
    }
    private void Update()
    {
        if(time >= 1)
        {
            GetDamage(500);
            time = 0;
        }
        else
        {
            time += Time.deltaTime;
        }
    }
    public void InitHP()
    {
        _currentHealthPoint = base.MaxHealthPoint;
        _currentHealthPointRatio = 100;
    }
    public void GetDamage(int damage) // 피격 판정 시 호출
    {
        int damagedHealthPoint = _currentHealthPoint - damage;
        _currentHealthPoint = Mathf.Max(damagedHealthPoint, DEAD_TRIGGER_HP);
        _currentHealthPointRatio = (_currentHealthPoint * 100) / base.MaxHealthPoint;
        if (_currentHealthPoint == DEAD_TRIGGER_HP)
        {
            this.gameObject.SetActive(false);
            OnPlayerDie.Invoke();
        }
        OnPlayerHealthPointChange.Invoke(_currentHealthPoint, _currentHealthPointRatio);
    }
}

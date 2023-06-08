using UnityEngine;
public class CharacterStatus : CharacterDefaultStatus
{
    // ���� �� �ٲ� �� �ִ� ���ȵ�
    public int HealthPoint { get => _currentHealthPoint; set => _currentHealthPoint = value; }
    public int HealthPointRatio { get => _currentHealthPointRatio; set => _currentHealthPointRatio = value; }
    public TeamType TeamType { get => _teamType; set => _teamType = value; }

    private int _currentHealthPoint;
    private int _currentHealthPointRatio;
    
    private const int DEAD_TRIGGER_HP = 0;

    private TeamType _teamType;

    private void Awake()
    {
        InitHP();
    }
    public void InitHP()
    {
        _currentHealthPoint = base.MaxHealthPoint;
    }
    public void GetDamage(int damage) // �ǰ� ���� �� ȣ��
    {
        int damagedHealthPoint = _currentHealthPoint - damage;
        _currentHealthPoint = Mathf.Max(damagedHealthPoint, DEAD_TRIGGER_HP);
        _currentHealthPointRatio = (_currentHealthPoint * 100) / base.MaxHealthPoint;
        if (_currentHealthPoint == DEAD_TRIGGER_HP)
        {
            this.gameObject.SetActive(false);
        }
    }
}

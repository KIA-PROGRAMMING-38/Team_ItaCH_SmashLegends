using UnityEngine;

public class AttackDamage : MonoBehaviour, IDamage
{
    // TODO 스탯 연동 시 데이터 연결
    private LegendController _legendController;
    
    protected int damage;

    private void Awake()
    {
        _legendController = GetComponent<LegendController>();
    }

    public int GetDamage()
    {
        // _legendController.Stat.Damage;

        return damage;
    }
}

public class LegendStatData
{
    public string LegendNameKOR { get; set; }
    public int LegendID { get; set; }
    public int SkillGauge { get; set; }
    public int SkillGaugeRecovery { get; set; }
    public float MoveSpeed { get; set; }
    public float JumpAcceleration { get; set; }
    public float GravitationalAcceleration { get; set; }
    public int MaxFallingSpeed { get; set; }
    public int Size { get; set; }
    public int HP { get; set; }
    public int DefaultAttackDamage { get; set; }
    public int JumpAttackDamage { get; set; }
    public int HeavyAttackDamage { get; set; }
    public int SkillAttackDamage { get; set; }
    public float DashPower { get; set; }
    public float DefaultKnockbackPower { get; set; }
    public float HeavyKnockbackPower { get; set; }
    public float HeavyCooltime { get; set; }

    public LegendStatData Clone()
    {
        return (LegendStatData)this.MemberwiseClone();
    }

}
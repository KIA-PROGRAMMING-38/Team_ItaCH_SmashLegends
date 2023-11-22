using CsvHelper.Expressions;
using UnityEngine.InputSystem;

public class LegendStatData : DataManager.ICsvParsable
{
    enum Fields
    {
        LegendNameKOR,
        LegendID,
        SkillGauge,
        SkillGaugeRecovery,
        MoveSpeed,
        JumpAcceleration,
        GravitationalAcceleration,
        MaxFallingSpeed,
        Size,
        HP,
        DefaultAttackDamage,
        JumpAttackDamage,
        HeavyAttackDamage,
        SkillAttackDamage,
        DashPower,
        DefaultKnockbackPower,
        HeavyKnockbackPower,
        HeavyCooltime
    }

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

    public void Parse(DataManager.CsvItem[] row)
    {
        LegendNameKOR = row[(int)Fields.LegendNameKOR].ToString();
        LegendID = row[(int)Fields.LegendID].ToInt();
        SkillGauge = row[(int)Fields.SkillGauge].ToInt();
        SkillGaugeRecovery = row[(int)Fields.SkillGaugeRecovery].ToInt();
        MoveSpeed = row[(int)Fields.MoveSpeed].ToFloat();
        JumpAcceleration = row[(int)Fields.JumpAcceleration].ToFloat();
        GravitationalAcceleration = row[(int)Fields.GravitationalAcceleration].ToFloat();
        MaxFallingSpeed = row[(int)Fields.MaxFallingSpeed].ToInt();
        Size = row[(int)Fields.Size].ToInt();
        HP = row[(int)Fields.HP].ToInt();
        DefaultAttackDamage = row[(int)Fields.DefaultAttackDamage].ToInt();
        JumpAttackDamage = row[(int)Fields.JumpAttackDamage].ToInt();
        HeavyAttackDamage = row[(int)Fields.HeavyAttackDamage].ToInt();
        SkillAttackDamage = row[(int)Fields.SkillAttackDamage].ToInt();
        DashPower = row[(int)Fields.DashPower].ToFloat();
        DefaultKnockbackPower = row[(int)Fields.DefaultKnockbackPower].ToFloat();
        HeavyKnockbackPower = row[(int)Fields.HeavyKnockbackPower].ToFloat();
        HeavyCooltime = row[(int)(Fields.HeavyCooltime)].ToFloat();
    }

    public LegendStatData Clone()
    {
        return (LegendStatData)this.MemberwiseClone();
    }

}
using UnityEngine;
using Util.Enum;

public class CharacterDefaultStatus : MonoBehaviour
{
    internal DataTable dataTable;

    protected int LegendIdData;
    protected int SkillGaugeData;
    protected int SkillRecoveryData;
    protected float MoveSpeedData;
    protected float JumpAccelerationData;
    protected float GravitationalAccelerationData;
    protected int MaxFallingSpeedData;
    protected int SizeData;
    protected int HpData;
    protected int DefaultAttackDamageData;
    protected int JumpAttackDamageData;
    protected int HeavyAttackDamageData;
    protected int SkillAttackDamageData;
    protected float DashPowerData;
    protected float DefaultKnockbackPowerData;
    protected float HeavyKnockbackPowerData;
    protected float HeavyCooltimeData;

    public void InitCharacterStatus(CharacterType id)
    {
        LegendIdData = dataTable.CharacterTable[(int)id].legendId;
        SkillGaugeData = dataTable.CharacterTable[(int)id].skillGauge;
        SkillRecoveryData = dataTable.CharacterTable[(int)id].skillRecovery;
        MoveSpeedData = dataTable.CharacterTable[(int)id].moveSpeed;
        JumpAccelerationData = dataTable.CharacterTable[(int)id].jumpAcceleration;
        GravitationalAccelerationData = dataTable.CharacterTable[(int)id].gravitationalAcceleration;
        MaxFallingSpeedData = dataTable.CharacterTable[(int)id].maxFallingSpeed;
        SizeData = dataTable.CharacterTable[(int)id].size;
        HpData = dataTable.CharacterTable[(int)id].hp;
        DefaultAttackDamageData = dataTable.CharacterTable[(int)id].defaultAttackDamage;
        HeavyAttackDamageData = dataTable.CharacterTable[(int)id].heavyAttackDamage;
        SkillAttackDamageData = dataTable.CharacterTable[(int)id].skillAttackDamage;
        DashPowerData = dataTable.CharacterTable[(int)id].dashPower;
        DefaultKnockbackPowerData = dataTable.CharacterTable[(int)id].defaultKnockbackPower;
        HeavyKnockbackPowerData = dataTable.CharacterTable[(int)id].heavyKnockbackPower;
        HeavyCooltimeData = dataTable.CharacterTable[(int)id].heavyCooltime;
    }
}

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

    public void GetCharacterDefaultData(int characterID)
    {
        dataTable = GameManager.Instance.CharacterTable;
        
        LegendIdData = dataTable.CharacterTable[characterID].legendId;        
        SkillGaugeData = dataTable.CharacterTable[characterID].skillGauge;
        SkillRecoveryData = dataTable.CharacterTable[characterID].skillRecovery;
        MoveSpeedData = dataTable.CharacterTable[characterID].moveSpeed;        
        JumpAccelerationData = dataTable.CharacterTable[characterID].jumpAcceleration;
        GravitationalAccelerationData = dataTable.CharacterTable[characterID].gravitationalAcceleration;
        MaxFallingSpeedData = dataTable.CharacterTable[characterID].maxFallingSpeed;
        SizeData = dataTable.CharacterTable[characterID].size;
        HpData = dataTable.CharacterTable[characterID].hp;
        DefaultAttackDamageData = dataTable.CharacterTable[characterID].defaultAttackDamage;
        HeavyAttackDamageData = dataTable.CharacterTable[characterID].heavyAttackDamage;
        SkillAttackDamageData = dataTable.CharacterTable[characterID].skillAttackDamage;
        DashPowerData = dataTable.CharacterTable[characterID].dashPower;
        DefaultKnockbackPowerData = dataTable.CharacterTable[characterID].defaultKnockbackPower;
        HeavyKnockbackPowerData = dataTable.CharacterTable[characterID].heavyKnockbackPower;
        HeavyCooltimeData = dataTable.CharacterTable[characterID].heavyCooltime;
    }
}

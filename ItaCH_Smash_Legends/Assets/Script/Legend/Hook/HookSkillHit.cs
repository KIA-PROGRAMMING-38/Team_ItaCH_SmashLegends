public class HookSkillHit : HookHit
{
    protected override void CalculationPowerAndDamage()
    {
        defaultdamage = skillDamage;
        knockbackUpDirection = skillKnockbackUpDirection;
        knockbackPower = defaultKnockbackPower;
    }
}

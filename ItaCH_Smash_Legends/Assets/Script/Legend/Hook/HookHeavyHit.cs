public class HookHeavyHit : HookHit
{
    protected override void CalculationPowerAndDamage()
    {
        defaultdamage = heavyDamage;
        knockbackPower = heavyKnockbackPower;
        knockbackUpDirection = heavyKnockbackUpDirection;
        animationHashValue = AnimationHash.HitUp;
    }
}

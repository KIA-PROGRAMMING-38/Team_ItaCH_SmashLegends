public class HookHeavyHitZone : HookHitZone
{
    private void Start()
    {
        base.Start();
        damageAmount = (int)(legendController.Stat.HeavyAttackDamage * 0.6f);
        knockbackPower = legendController.Stat.HeavyKnockbackPower;
        animationType = AnimationHash.HitUp;
        knockbackUpDirection = heavyKnockbackUpDirection;
    }
}

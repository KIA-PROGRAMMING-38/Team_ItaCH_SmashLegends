public class HookFinishDefaultHitZone : HookHitZone
{
    private void Start()
    {
        base.Start();
        damageAmount = (int)(legendController.Stat.DefaultAttackDamage * 0.4f);
        knockbackPower = legendController.Stat.HeavyKnockbackPower;
        animationType = AnimationHash.HitUp;
        knockbackUpDirection = heavyKnockbackUpDirection;
    }
}

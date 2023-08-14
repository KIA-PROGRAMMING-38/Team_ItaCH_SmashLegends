public class HookDefaultHitZone : HookHitZone
{
    private void Start()
    {
        base.Start();
        damageAmount = (int)(legendController.Stat.DefaultAttackDamage * 0.2f);
        knockbackPower = legendController.Stat.DefaultKnockbackPower;
        animationType = AnimationHash.Hit;
    }
}

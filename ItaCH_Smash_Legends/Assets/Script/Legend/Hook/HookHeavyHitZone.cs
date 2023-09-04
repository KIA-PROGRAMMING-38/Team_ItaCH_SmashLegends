public class HookHeavyHitZone : HookHitZone
{
    private void Start()
    {
        base.Start();
        DamageAmount = (int)(legendController.Stat.HeavyAttackDamage * 0.6f);
        knockbackPower = legendController.Stat.HeavyKnockbackPower;
        AnimationType = AnimationHash.HitUp;
        AttackSound = StringLiteral.SFX_HEAVYATTACK_HIT;
        knockbackUpDirection = heavyKnockbackUpDirection;
    }
}

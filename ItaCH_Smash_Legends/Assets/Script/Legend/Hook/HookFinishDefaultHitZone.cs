public class HookFinishDefaultHitZone : HookHitZone
{
    private void Start()
    {
        base.Start();
        DamageAmount = (int)(legendController.Stat.DefaultAttackDamage * 0.4f);
        knockbackPower = legendController.Stat.HeavyKnockbackPower;
        AttackSound = StringLiteral.SFX_DEFAULTATTACK_HIT;
        AnimationType = AnimationHash.HitUp;
        knockbackUpDirection = heavyKnockbackUpDirection;
    }
}

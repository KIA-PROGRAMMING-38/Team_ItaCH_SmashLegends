public class HookDefaultHitZone : HookHitZone
{
    private void Start()
    {
        base.Start();
        DamageAmount = (int)(legendController.Stat.DefaultAttackDamage * 0.2f);
        knockbackPower = legendController.Stat.DefaultKnockbackPower;
        AttackSound = StringLiteral.SFX_DEFAULTATTACK_HIT;
        AnimationType = AnimationHash.Hit;
    }
}

public class HookSkillHitZone : HookHitZone
{
    private void Start()
    {
        base.Start();
        DamageAmount = legendController.Stat.SkillAttackDamage;
        knockbackPower = legendController.Stat.DefaultKnockbackPower;
        AnimationType = AnimationHash.Hit;
        AttackSound = StringLiteral.SFX_SKILLATTACK_HIT;
        knockbackUpDirection = skillKnockbackUpDirection;
    }
}

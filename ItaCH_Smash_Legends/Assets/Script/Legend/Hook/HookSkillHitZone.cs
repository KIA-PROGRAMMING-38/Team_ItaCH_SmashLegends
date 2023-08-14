public class HookSkillHitZone : HookHitZone
{
    private void Start()
    {
        base.Start();
        damageAmount = legendController.Stat.SkillAttackDamage;
        knockbackPower = legendController.Stat.DefaultKnockbackPower;
        animationType = AnimationHash.Hit;
        knockbackUpDirection = skillKnockbackUpDirection;
    }
}

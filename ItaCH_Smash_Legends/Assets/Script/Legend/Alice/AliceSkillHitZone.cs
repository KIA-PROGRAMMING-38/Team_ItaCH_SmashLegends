public class AliceSkillHitZone : HitZone
{
    private void Start()
    {
        DamageAmount = legendController.Stat.SkillAttackDamage;
        knockbackPower = legendController.Stat.HeavyKnockbackPower * 2;
        knockbackDirection = transform.up;
        AnimationType = AnimationHash.HitUp;
        gameObject.SetActive(false);
    }
}

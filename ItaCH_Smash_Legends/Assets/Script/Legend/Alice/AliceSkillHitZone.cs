public class AliceSkillHitZone : HitZone
{
    private void Start()
    {
        damageAmount = legendController.Stat.SkillAttackDamage;
        knockbackPower = legendController.Stat.HeavyKnockbackPower * 2;
        knockbackDirection = transform.up;
        animationType = AnimationHash.HitUp;
        gameObject.SetActive(false);
    }
}

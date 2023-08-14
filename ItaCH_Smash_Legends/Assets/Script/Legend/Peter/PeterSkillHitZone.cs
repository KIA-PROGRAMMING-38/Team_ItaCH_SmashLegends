public class PeterSkillHitZone : HitZone
{
    private void Start()
    {
        damageAmount = (int)(legendController.Stat.SkillAttackDamage * 0.1f);
        knockbackPower = legendController.Stat.DefaultKnockbackPower;
        animationType = AnimationHash.Hit;
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        knockbackDirection = transform.forward;
    }
}

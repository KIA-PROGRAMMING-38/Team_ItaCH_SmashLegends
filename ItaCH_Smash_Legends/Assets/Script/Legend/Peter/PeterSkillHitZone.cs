public class PeterSkillHitZone : HitZone
{
    private void Start()
    {
        DamageAmount = (int)(legendController.Stat.SkillAttackDamage * 0.1f);
        knockbackPower = legendController.Stat.DefaultKnockbackPower;
        AnimationType = AnimationHash.Hit;
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        knockbackDirection = transform.forward;
    }
}

public class PeterSkillHitZone : HitZone
{
    private void Start()
    {
        DamageAmount = (int)(legendController.Stat.SkillAttackDamage * 0.1f);
        knockbackPower = legendController.Stat.DefaultKnockbackPower;
        AttackSound = StringLiteral.SFX_SKILLATTACK_HIT;
        AnimationType = AnimationHash.Hit;
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        knockbackDirection = transform.forward;
    }
}

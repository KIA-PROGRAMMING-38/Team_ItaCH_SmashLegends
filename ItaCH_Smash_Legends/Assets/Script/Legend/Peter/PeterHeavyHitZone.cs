public class PeterHeavyHitZone : HitZone
{
    private void Start()
    {
        damageAmount = legendController.Stat.HeavyAttackDamage;
        knockbackPower = legendController.Stat.HeavyKnockbackPower;
        animationType = AnimationHash.HitUp;
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        knockbackDirection = transform.forward + transform.up;
    }
}

public class PeterFinishAttackHitZone : HitZone
{
    private void Start()
    {
        damageAmount = (int)(legendController.Stat.DefaultAttackDamage * 0.4f);
        knockbackPower = legendController.Stat.HeavyKnockbackPower;
        animationType = AnimationHash.HitUp;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        knockbackDirection = transform.forward;
    }
}


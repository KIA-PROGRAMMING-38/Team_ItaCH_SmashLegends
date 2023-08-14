public class AliceFinishHitZone : HitZone
{
    private void Start()
    {
        damageAmount = (int)(legendController.Stat.DefaultAttackDamage * 0.6f);
        knockbackPower = legendController.Stat.HeavyKnockbackPower;
        animationType = AnimationHash.HitUp;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        knockbackDirection = transform.forward + transform.up;
    }
}

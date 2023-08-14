public class AliceFinishHitZone : HitZone
{
    private void Start()
    {
        DamageAmount = (int)(legendController.Stat.DefaultAttackDamage * 0.6f);
        knockbackPower = legendController.Stat.HeavyKnockbackPower;
        AnimationType = AnimationHash.HitUp;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        knockbackDirection = transform.forward + transform.up;
    }
}

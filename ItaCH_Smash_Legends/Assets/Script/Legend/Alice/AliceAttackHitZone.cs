public class AliceAttackHitZone : HitZone
{
    private void Start()
    {
        DamageAmount = (int)(legendController.Stat.DefaultAttackDamage * 0.4);
        knockbackPower = legendController.Stat.DefaultKnockbackPower;
        AnimationType = AnimationHash.Hit;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        knockbackDirection = transform.forward + transform.up;
    }
}

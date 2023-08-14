public class AliceJumpHitZone : HitZone
{
    private void Start()
    {
        damageAmount = legendController.Stat.JumpAttackDamage;
        knockbackPower = legendController.Stat.HeavyKnockbackPower;
        animationType = AnimationHash.HitUp;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        knockbackDirection = transform.forward + transform.up;
    }
}

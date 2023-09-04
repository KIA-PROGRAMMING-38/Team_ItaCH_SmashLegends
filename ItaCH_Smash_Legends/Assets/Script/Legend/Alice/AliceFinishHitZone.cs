public class AliceFinishHitZone : HitZone
{
    private void Start()
    {
        DamageAmount = (int)(legendController.Stat.DefaultAttackDamage * 0.6f);
        knockbackPower = legendController.Stat.HeavyKnockbackPower;
        AnimationType = AnimationHash.HitUp;
        AttackSound = StringLiteral.SFX_DEFAULTATTACK_HIT;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        knockbackDirection = transform.forward + transform.up;
    }
}

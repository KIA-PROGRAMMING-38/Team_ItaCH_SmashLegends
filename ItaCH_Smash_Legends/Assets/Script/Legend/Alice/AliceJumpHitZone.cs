public class AliceJumpHitZone : HitZone
{
    private void Start()
    {
        DamageAmount = legendController.Stat.JumpAttackDamage;
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

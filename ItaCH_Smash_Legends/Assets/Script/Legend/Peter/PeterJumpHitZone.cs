public class PeterJumpHitZone : HitZone
{
    private void Start()
    {
        DamageAmount = legendController.Stat.JumpAttackDamage;
        knockbackPower = legendController.Stat.HeavyKnockbackPower;
        AttackSound = StringLiteral.SFX_DEFAULTATTACK_HIT;
        AnimationType = AnimationHash.HitUp;
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        knockbackDirection = transform.forward + transform.up;
    }
}

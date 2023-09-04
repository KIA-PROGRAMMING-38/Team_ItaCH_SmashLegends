public class PeterFinishAttackHitZone : HitZone
{
    private void Start()
    {
        DamageAmount = (int)(legendController.Stat.DefaultAttackDamage * 0.4f);
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


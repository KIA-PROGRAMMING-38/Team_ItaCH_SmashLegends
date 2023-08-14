public class PeterAttackHitZone : HitZone
{
    private void Start()
    {
        // TODO : 스탯 연동후 재설정 
        DamageAmount = (int)(legendController.Stat.DefaultAttackDamage * 0.3f);
        knockbackPower = legendController.Stat.DefaultKnockbackPower;
        AnimationType = AnimationHash.Hit;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        knockbackDirection = transform.forward;
    }
}

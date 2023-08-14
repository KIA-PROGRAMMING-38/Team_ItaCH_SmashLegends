public class PeterAttackHitZone : HitZone
{
    private void Start()
    {
        // TODO : 스탯 연동후 재설정 
        damageAmount = (int)(legendController.Stat.DefaultAttackDamage * 0.3f);
        knockbackPower = legendController.Stat.DefaultKnockbackPower;
        animationType = AnimationHash.Hit;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        knockbackDirection = transform.forward;
    }
}

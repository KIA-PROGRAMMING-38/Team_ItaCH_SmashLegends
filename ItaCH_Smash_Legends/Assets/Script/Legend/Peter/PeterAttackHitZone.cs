public class PeterAttackHitZone : HitZone
{
    void Start()
    {
        // TODO : 스탯 연동후 재설정 
        damageAmount = 100;
        knockbackPower = 0.6f;
        animationType = AnimationHash.Hit;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        knockbackDirection = transform.forward;
    }
}

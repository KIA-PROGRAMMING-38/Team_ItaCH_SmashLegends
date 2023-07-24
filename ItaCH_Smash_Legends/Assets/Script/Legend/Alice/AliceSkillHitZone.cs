public class AliceSkillHitZone : HitZone
{
    void Start()
    {
        // TODO : 스탯 연동후 재설정 
        damageAmount = 1000;
        knockbackPower = 1.5f;
        knockbackDirection = transform.up;
        animationType = AnimationHash.HitUp;
        gameObject.SetActive(false);
    }
}

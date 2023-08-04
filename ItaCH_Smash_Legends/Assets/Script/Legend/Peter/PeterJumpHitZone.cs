public class PeterJumpHitZone : HitZone
{
    // Start is called before the first frame update
    void Start()
    {
        // TODO : 스탯 연동후 재설정 
        damageAmount = 100;
        knockbackPower = 0.8f;
        animationType = AnimationHash.HitUp;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        knockbackDirection = transform.forward + transform.up;
    }
}
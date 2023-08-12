public class HookDefaultHitZone : HookHitZone
{
    private void Start()
    {
        base.Start();
        // TODO : 스탯 연동후 재설정 
        damageAmount = 100;
        knockbackPower = 0.05f;
        animationType = AnimationHash.Hit;
    }
}

public class HookFinishDefaultHitZone : HookHitZone
{
    private void Start()
    {
        base.Start();
        // TODO : 스탯 연동후 재설정 
        damageAmount = 200;
        knockbackPower = 0.7f;
        animationType = AnimationHash.HitUp;
        knockbackUpDirection = heavyKnockbackUpDirection;
    }
}

public class HookSkillHitZone : HookHitZone
{
    private void Start()
    {
        base.Start();
        // TODO : 스탯 연동후 재설정 
        damageAmount = 50;
        knockbackPower = 0.05f;
        animationType = AnimationHash.Hit;
        knockbackUpDirection = skillKnockbackUpDirection;
    }
}

public class HookHeavyHit : HookHit
{
    void Start()
    {
        damage = heavyDamage;
        knockbackPower = heavyKnockbackPower;
        knockbackUpDirection = heavyKnockbackUpDirection;
        animationHashValue = AnimationHash.HitUp;
    }
}

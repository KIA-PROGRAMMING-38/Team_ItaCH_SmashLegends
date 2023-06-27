public class HookHeavyHit : HookHit
{
    void Start()
    {
        defaultdamage = heavyDamage;
        knockbackPower = heavyKnockbackPower;
        knockbackUpDirection = heavyKnockbackUpDirection;
        animationHashValue = AnimationHash.HitUp;
    }
}

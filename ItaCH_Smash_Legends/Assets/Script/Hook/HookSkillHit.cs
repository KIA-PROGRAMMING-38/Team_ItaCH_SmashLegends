public class HookSkillHit : HookHit
{
    void Start()
    {
        defaultdamage = skillDamage;
        knockbackUpDirection = skillKnockbackUpDirection;
        knockbackPower = defaultKnockbackPower;
    }
}

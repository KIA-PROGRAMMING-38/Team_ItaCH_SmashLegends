public class HookSkillHit : HookHit
{
    void Start()
    {
        damage = skillDamage;
        knockbackUpDirection = skillKnockbackUpDirection;
        knockbackPower = lightKnockbackPower;
    }
}

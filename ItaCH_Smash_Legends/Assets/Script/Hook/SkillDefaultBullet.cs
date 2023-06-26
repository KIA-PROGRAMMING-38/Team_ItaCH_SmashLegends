public class SkillDefaultBullet : HookBullet
{
    private void Awake()
    {
        bulletDeleteEffectPath = SKILL_BULLET_DELETE_EFFECT_PATH;
        bulletDeleteTime = SKILL_BULLET_DELETE_TIME;
        currentBulletSpeed = DEFAULT_BULLET_SPEED;
    }
}

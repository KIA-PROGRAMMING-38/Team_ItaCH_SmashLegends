public class DefaultBullet : HookBullet
{    private void Awake()
    {
        bulletDeleteTime = DEFAULT_BULLET_DELETE_TIME;
        currentBulletSpeed = DEFAULT_BULLET_SPEED;
    }
}

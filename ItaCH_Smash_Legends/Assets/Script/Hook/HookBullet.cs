using UnityEngine;
using UnityEngine.Pool;

public class HookBullet : MonoBehaviour
{
    protected BulletDeleteEffect bulletDeleteEffect;
    protected const float DEFAULT_BULLET_SPEED = 20f;
    protected const float HEAVY_BULLET_SPEED = 28f;
    protected const float DEFAULT_BULLET_DELETE_TIME = 0.23f;
    protected const float HEAVY_BULLET_DELETE_TIME = 0.28f;
    protected const float SKILL_BULLET_DELETE_TIME = 0.28f;
    protected const float SKILL_HEAVY_BULLET_DELETE_TIME = 0.3f;
    protected readonly string HEAVY_BULLET_DELETE_EFFECT_PATH = ResourcesPath.HeavyBulletDeleteEffect;
    protected readonly string LAST_HEAVY_BULLET_DELETE_EFFECT_PATH = ResourcesPath   .LastHeavyBulletDeleteEffect;
    protected readonly string SKILL_BULLET_DELETE_EFFECT_PATH = ResourcesPath.SkillBulletDeleteEffect;

    protected float bulletDeleteTime = 0.23f;
    protected float currentBulletSpeed = 20;

    protected string bulletDeleteEffectPath = ResourcesPath.BulletDeleteEffect;

    public ObjectPool<HookBullet> Pool { get; set; }
    internal GameObject constructor;

    private ObjectPool<BulletDeleteEffect> _bulletDeleteEffectPool;
    private float _elapsedTime;

    private void Start()
    {
        bulletDeleteEffect = Resources.Load<BulletDeleteEffect>(bulletDeleteEffectPath);
        _bulletDeleteEffectPool = new ObjectPool<BulletDeleteEffect>(CreateBulletDeleteEffectOnPool,
            GetPoolBulletDeleteEffect, ReturnBulletDeleteEffect, (effect) => Destroy(effect), true, 10, 500);
    }

    private void Update()
    {
        BulletDirection();
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= bulletDeleteTime)
        {
            BulletPostProcessing(transform.position);
        }
    }

    private void BulletDirection()
    {
        transform.Translate(Vector3.forward * (currentBulletSpeed * Time.deltaTime));
    }
    public void BulletPostProcessing(Vector3 position)
    {
        Pool.Release(this);
        BulletDeleteEffect effect = _bulletDeleteEffectPool.Get();
        effect.transform.position = position;
        _elapsedTime = 0;
    }
    private BulletDeleteEffect CreateBulletDeleteEffectOnPool()
    {
        BulletDeleteEffect effect = Instantiate(bulletDeleteEffect);
        effect.Pool = _bulletDeleteEffectPool;
        return effect;
    }
    private void GetPoolBulletDeleteEffect(BulletDeleteEffect effect) => effect.gameObject.SetActive(true);
    private void ReturnBulletDeleteEffect(BulletDeleteEffect effect) => effect.gameObject.SetActive(false);
}

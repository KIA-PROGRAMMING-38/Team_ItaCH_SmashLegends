using UnityEngine;

public class HookHitZone : HitZone
{
    protected float defaultKnockbackPower = 0.05f;
    protected float heavyKnockbackPower = 0.7f;
    protected Vector3 heavyKnockbackUpDirection = new Vector3(0, 0.7f, 0);
    protected Vector3 skillKnockbackUpDirection = new Vector3(0, 0.15f, 0);
    protected Vector3 knockbackUpDirection = new Vector3(0, 0.3f, 0);

    private HookBullet _hookBullet;

    protected void Start()
    {
        _hookBullet = GetComponentInParent<HookBullet>();
        legendController = _hookBullet.constructor.GetComponent<LegendController>();
        gameObject.layer = LayerMask.NameToLayer(legendController.GetChildLayer());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject.layer != _hookBullet.constructor.layer)
        {
            knockbackDirection = knockbackUpDirection + _hookBullet.transform.forward;
            _hookBullet.BulletPostProcessing(GetBulletDeleteEffectPosition(other));
        }
    }

    private Vector3 GetBulletDeleteEffectPosition(Collider other)
    {
        Vector3 bulletHitPosition;

        bulletHitPosition.x = other.transform.position.x;
        bulletHitPosition.y = transform.position.y;
        bulletHitPosition.z = other.transform.position.z;

        return bulletHitPosition;
    }
}

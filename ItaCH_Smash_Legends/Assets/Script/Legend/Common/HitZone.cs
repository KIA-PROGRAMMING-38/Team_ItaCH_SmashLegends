using UnityEngine;

public class HitZone : MonoBehaviour
{
    protected LegendController legendController;

    protected float knockbackPower;
    protected Vector3 knockbackDirection;
    public int DamageAmount { get; protected set; }
    public int AnimationType { get; protected set; }

    public void SetKnockback(Rigidbody rigidbody)
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(knockbackDirection * knockbackPower, ForceMode.Impulse);
    }
    protected void Awake()
    {
        legendController = transform.GetComponentInParent<LegendController>();
    }
}

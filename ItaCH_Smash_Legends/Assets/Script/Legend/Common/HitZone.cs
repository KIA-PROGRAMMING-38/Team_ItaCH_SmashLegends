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
        Vector3 direction = knockbackDirection;
        rigidbody.AddForce(direction * knockbackPower, ForceMode.Impulse);
    }
    protected void Awake()
    {
        legendController = transform.GetComponentInParent<LegendController>();
    }
    // TODO : 스탯 연동 후 GetDamage() => legendController.Stat.HP -= damageAmount;
    public int GetDamage()
    {
        return DamageAmount;
    }

    public int GetAnimationKind()
    {
        return AnimationType;
    }
}

using UnityEngine;

public class HitZone : MonoBehaviour
{
    protected LegendController legendController;

    protected int damageAmount;
    protected float knockbackPower;
    protected Vector3 knockbackDirection;
    protected int animationType;

    public void SetKnockback(Rigidbody rigidbody)
    {
        Vector3 direction = knockbackDirection;
        rigidbody.AddForce(direction * knockbackPower , ForceMode.Impulse);
    }
    // TODO : 스탯 연동 후 GetDamage() => legendController.Stat.HP -= damageAmount;
    public int GetDamage()
    {
        return damageAmount;
    }

    public int GetAnimationKind()
    {
        return animationType;
    }
}

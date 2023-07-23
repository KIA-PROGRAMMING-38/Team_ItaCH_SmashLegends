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
        rigidbody.AddForce(knockbackDirection * knockbackPower , ForceMode.Impulse);
    }
    // TODO : 스탯 연동 후 Damage() => legendController.Stat.HP -= damage;
    public int Damage()
    {
        return damageAmount;
    }

    public int GetAnimationKind()
    {
        return animationType;
    }
}

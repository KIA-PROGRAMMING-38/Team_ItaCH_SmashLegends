using UnityEngine;

public class HitZone : MonoBehaviour
{
    protected LegendController legendController;

    protected float knockbackPower;
    protected Vector3 knockbackDirection;
    public int DamageAmount { get; protected set; }
    public int AnimationType { get; protected set; }
    public string AttackSound; 

    public void SetKnockback(Rigidbody rigidbody)
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(knockbackDirection * knockbackPower, ForceMode.Impulse);
    }
    public void PlaySFXAttackSound()
    {
        Managers.SoundManager.Play(SoundType.SFX, AttackSound,legendController.LegendType);
    }
    protected void Awake()
    {
        legendController = transform.GetComponentInParent<LegendController>();
    }
}

using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    protected LegendController legendController;
    protected Rigidbody attackRigidbody;
    protected float dashPower;

    private void Awake()
    {
        attackRigidbody = GetComponent<Rigidbody>();
        legendController = GetComponent<LegendController>();
    }
    private void DashOnAnimationEvent()
    {
        attackRigidbody.AddForce(transform.forward * dashPower, ForceMode.Impulse);
    }
}

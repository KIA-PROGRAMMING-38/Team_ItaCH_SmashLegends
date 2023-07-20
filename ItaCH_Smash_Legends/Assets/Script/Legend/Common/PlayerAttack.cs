using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    protected LegendController legendController;
    protected Rigidbody attackRigidbody;
    private void Awake()
    {
        legendController = GetComponent<LegendController>();
        attackRigidbody= GetComponent<Rigidbody>();
    }
    public virtual void DashOnAnimationEvent()
    {

    }
}

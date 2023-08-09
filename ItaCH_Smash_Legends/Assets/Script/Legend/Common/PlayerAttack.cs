using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    protected LegendController legendController;
    protected Rigidbody attackRigidbody;
    protected float dashPower = 1f;

    private void Awake()
    {
        //TODO : 스탯 연동 후 설정
        attackRigidbody = GetComponent<Rigidbody>();
        legendController = GetComponent<LegendController>();
        //dashPower = characterStatus.Stat.DashPower;
    }
    private void DashOnAnimationEvent()
    {
        attackRigidbody.AddForce(transform.forward * dashPower, ForceMode.Impulse);
    }
}

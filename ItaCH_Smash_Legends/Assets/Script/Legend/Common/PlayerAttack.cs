using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    protected LegendController legendController;
    protected Rigidbody attackRigidbody;
    protected float dashPower = 1f;

    private void Awake()
    {
        //TODO : 스탯 연동 후 설정
        //dashPower = characterStatus.Stat.DashPower;
        characterStatus = GetComponent<CharacterStatus>();
        attackRigidbody = GetComponent<Rigidbody>();
        legendController = GetComponent<LegendController>();
    }
    private void DashOnAnimationEvent()
    {
        attackRigidbody.AddForce(transform.forward * dashPower, ForceMode.Impulse);
    }
}

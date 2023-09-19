using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    enum AttackType
    {
        Default = 0,
        Jump = 6,
        Last = 7
    }
    protected LegendController legendController;
    protected Rigidbody attackRigidbody;
    protected float dashPower;

    public bool CanHeavyAttack;
    public bool CanSkillAttack;

    private void Awake()
    {
        attackRigidbody = GetComponent<Rigidbody>();
        legendController = GetComponent<LegendController>();
    }
    private void DashOnAnimationEvent()
    {
        attackRigidbody.AddForce(transform.forward * dashPower, ForceMode.Impulse);
    }
    private void PlayAttackVoiceOnAnimationEvent(AttackType attackType)
    {
        Managers.SoundManager.Play(SoundType.Voice, legend: legendController.LegendType, voice: (VoiceType)attackType);
    }
}

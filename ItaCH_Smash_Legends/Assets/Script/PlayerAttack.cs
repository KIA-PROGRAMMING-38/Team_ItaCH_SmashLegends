using UnityEngine;

public class PlayerAttack : MonoBehaviour, IAttack
{
    public readonly int MAX_POSSIBLE_ATTACK_COUNT = 3;
    public readonly int COMBO_SECOND_COUNT = 2;
    public readonly int COMBO_FINISH_COUNT = 1;

    public int CurrentPossibleComboCount;

    internal bool isFirstAttack;
    internal bool isSecondAttack;
    internal bool isFinishAttack;

    protected PlayerMove playerMove;
    protected Rigidbody rigidbodyAttack;
    protected CharacterStatus characterStatus;
    protected PlayerStatus playerStatus;
    protected Animator animator;

    protected float defaultDashPower = 1f;
    internal float nextTransitionMinValue = 0.5f;
    internal float nextTransitionMaxValue = 0.8f;
    protected float heavyCooltime;
    protected int skillGauage;
    protected int skillGauageRecovery;

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        rigidbodyAttack = GetComponent<Rigidbody>();
        characterStatus = GetComponent<CharacterStatus>();
        playerStatus = GetComponent<PlayerStatus>();
        animator = GetComponent<Animator>();

        CurrentPossibleComboCount = MAX_POSSIBLE_ATTACK_COUNT;
    }
    private void Start()
    {
        SetStatus();
    }
    private void SetStatus()
    {
        defaultDashPower = characterStatus.DashPower;
        heavyCooltime = characterStatus.HeavyCooltime;
        skillGauage = characterStatus.SkillGauage;
        skillGauageRecovery = characterStatus.SkillGauageRecovery;
    }
    public void AttackRotate()
    {
        if (playerMove.moveDirection != Vector3.zero)
        {
            transform.forward = playerMove.moveDirection;
        }
    }

    protected bool IsPossibleFirstAttack()
    {
        if (CurrentPossibleComboCount == MAX_POSSIBLE_ATTACK_COUNT)
        {
            if (playerStatus.CurrentState == PlayerStatus.State.Idle || playerStatus.CurrentState == PlayerStatus.State.Run)
            {
                return true;
            }
        }
        return false;
    }
    public virtual void AttackOnDash()
    {

    }

    public virtual void DefaultAttack()
    {
        if (IsPossibleFirstAttack())
        {
            isFinishAttack = false;
            animator.Play(AnimationHash.FirstAttack);
        }

        if (isFirstAttack && CurrentPossibleComboCount == COMBO_SECOND_COUNT)
        {
            isSecondAttack = true;
        }
        if (isSecondAttack && CurrentPossibleComboCount == COMBO_FINISH_COUNT)
        {
            isFinishAttack = true;
        }
    }

    public virtual void SkillAttack()
    {
        if (playerStatus.CurrentState == PlayerStatus.State.Run ||
            playerStatus.CurrentState == PlayerStatus.State.Idle ||
            playerStatus.CurrentState == PlayerStatus.State.Jump)
        {
            animator.Play(AnimationHash.SkillAttack);
            playerStatus.CurrentState = PlayerStatus.State.SkillAttack;
        }
    }

    public virtual void HeavyAttack()
    {
        if (playerStatus.CurrentState == PlayerStatus.State.Run ||
           playerStatus.CurrentState == PlayerStatus.State.Idle)
        {
            animator.Play(AnimationHash.HeavyAttack);
            playerStatus.CurrentState = PlayerStatus.State.HeavyAttack;
        }
    }

    public virtual void JumpAttack()
    {
        if (playerStatus.IsJump == false)
        {
            playerStatus.CurrentState = PlayerStatus.State.JumpAttack;
            animator.SetTrigger(AnimationHash.JumpAttack);
        }
    }

}

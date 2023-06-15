using UnityEngine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;

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

    private PlayerStatus _playerStatus;
    private Animator _animator;

    protected float _defaultDashPower = 1f;

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        rigidbodyAttack = GetComponent<Rigidbody>();
        _playerStatus = GetComponent<PlayerStatus>();
        _animator = GetComponent<Animator>();

        CurrentPossibleComboCount = MAX_POSSIBLE_ATTACK_COUNT;
    }

    public void AttackRotate()
    {
        if (playerMove.moveDirection != Vector3.zero)
        {
            transform.forward = playerMove.moveDirection;
        }
    }

    private bool IsPossibleFirstAttack()
    {
        if (CurrentPossibleComboCount == MAX_POSSIBLE_ATTACK_COUNT &&
         (_playerStatus.CurrentState == PlayerStatus.State.Run ||
         _playerStatus.CurrentState == PlayerStatus.State.Idle))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void AttackOnDash() => Debug.Log("재정의 필요");

    public virtual void DefaultAttack()
    {
        if (IsPossibleFirstAttack())
        {
            _animator.Play(AnimationHash.FirstAttack);
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
        if (_playerStatus.CurrentState == PlayerStatus.State.Run ||
            _playerStatus.CurrentState == PlayerStatus.State.Idle ||
            _playerStatus.CurrentState == PlayerStatus.State.Jump)
        {
            _animator.Play(AnimationHash.SkillAttack);
            _playerStatus.CurrentState = PlayerStatus.State.SkillAttack;
        }
    }

    public virtual void HeavyAttack()
    {
        if (_playerStatus.CurrentState == PlayerStatus.State.Run ||
           _playerStatus.CurrentState == PlayerStatus.State.Idle)
        {
            _animator.Play(AnimationHash.HeavyAttack);
            _playerStatus.CurrentState = PlayerStatus.State.HeavyAttack;
        }
    }

    public virtual void JumpAttack()
    {
        if (_playerStatus.IsJump == false)
        {
            _animator.SetTrigger(AnimationHash.JumpAttack);
            return;
        }
    }


}

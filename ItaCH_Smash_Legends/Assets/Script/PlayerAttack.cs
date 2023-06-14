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
    
    public Collider attackRange;

    protected float _defaultDashPower = 1f;

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        rigidbodyAttack = GetComponent<Rigidbody>();
        CurrentPossibleComboCount = MAX_POSSIBLE_ATTACK_COUNT;
    }
    public void AttackRotate()
    {
        if (playerMove.moveDirection != Vector3.zero)
        {
            transform.forward = playerMove.moveDirection;
        }
    }

    public virtual void AttackOnDash() => Debug.Log("재정의 필요");

    public virtual void DefaultAttack() => Debug.Log("재정의 필요");


    public virtual void SkillAttack() => Debug.Log("재정의 필요");

    public virtual void HeavyAttack() => Debug.Log("재정의 필요");

    public virtual void JumpAttack() => Debug.Log("재정의 필요");


}

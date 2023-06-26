using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public enum State
    {
        None, Idle, Run, Jump, Hang, HitUp, DownIdle, ComboAttack, FinishComboAttack, JumpAttack, HeavyAttack, SkillAttack, SkillEndAttack
    }

    public State CurrentState;
    public bool IsHit { get; set; }
    public bool IsAttack { get; set; }
    public bool IsJump { get; set; }
    public bool IsHang { get; set; }
    public bool IsRollUp { get; set; }

    private void Awake()
    {
        CurrentState = State.Idle;
    }
}
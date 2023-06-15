using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public enum State
    {
        None, Idle, Run, Jump, Hang, HitUp, ComboAttack, FinishComboAttack, HeavyAttack, SkillAttack, SkillEndAttack
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
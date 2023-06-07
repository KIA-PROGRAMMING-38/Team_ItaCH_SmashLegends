using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public enum State
    {
        None, Idle, Run, Jump, Hang, ComboAttack, HeavyAttack, SkillAttack
    }

    public State CurrentState;
    public bool IsHit { get; set; }
    public bool IsAttack { get; set; }
    public bool IsJump { get; set; }
    public bool IsHang { get; set; }

    private void Awake()
    {
        CurrentState = State.Idle;
    }
}
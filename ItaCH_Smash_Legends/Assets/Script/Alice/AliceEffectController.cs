using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliceEffectController : EffectController
{

    public enum EffectName
    {
        FirstAttackEffect,
        FirstAttackEndEffect,
        FinishAttackEffect,
        JumpAttackEffect,
        JumpSmokeEffect,
        SkillAttackStartEffect,
        SkillAttackEffect
    }
    public void EnableFirstAttackEffect() => _effects[(int)EffectName.FirstAttackEffect].SetActive(true);
    public void EnableFirstAttackEndEffect() => _effects[(int)EffectName.FirstAttackEndEffect].SetActive(true);
    public void EnableFinishAttack() => _effects[(int)EffectName.FinishAttackEffect].SetActive(true);
    public void EnableJumpAttackEffect() => _effects[(int)EffectName.JumpAttackEffect].SetActive(true);
    public void EnableJumpSmokeEffect() => _effects[(int)EffectName.JumpSmokeEffect].SetActive(true);
    public void EnableSkillAttackEffect() => _effects[(int)EffectName.SkillAttackEffect].SetActive(true);
    public void EnableSkillAttackStartEffect() => _effects[(int)EffectName.SkillAttackStartEffect].SetActive(true);
    public void DisableJumpAttackEffect() => _effects[(int)EffectName.JumpAttackEffect].SetActive(false);
    
}

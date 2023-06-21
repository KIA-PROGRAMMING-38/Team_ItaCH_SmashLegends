using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeterEffectController : EffectController
{
    private enum EffectName
    {
        FirstAttackEffect,
        SecondAttackFirstEffect,
        SecondAttackSecondEffect,
        FinalAttackEffect,
        HeavyAttackEffect,
        JumpAttackEffect,
        SkillAttackEffectStart,
        SkillAttackEffectMiddle,
        SkillAttackEffectEnd,
        AttackSmokeEffect,
        HeavySmokeEffect
    }
    public void EnableFirstAttackEffect() => _effects[(int)EffectName.FirstAttackEffect].SetActive(true);
    public void EnableSecondAttackFirstEffect() => _effects[(int)EffectName.SecondAttackFirstEffect].SetActive(true);
    public void EnableSecondAttackSecondEffect() => _effects[(int)EffectName.SecondAttackSecondEffect].SetActive(true);
    public void EnableFinishAttackEffect() => _effects[(int)EffectName.FinalAttackEffect].SetActive(true);
    public void EnableHeavyAttackEffect() => _effects[(int)EffectName.HeavyAttackEffect].SetActive(true);
    public void EnableJumpAttackEffect() => _effects[(int)EffectName.JumpAttackEffect].SetActive(true);
    public void DisableJumpAttackEffect() => _effects[(int)EffectName.JumpAttackEffect].SetActive(false);
    public void EnableSkillAttackEffect() => _effects[(int)EffectName.SkillAttackEffectStart].SetActive(true);
    public void EnableMiddleSkillAttackEffect() => _effects[(int)EffectName.SkillAttackEffectMiddle].SetActive(true);
    public void EnableEndSkillAttackEffect() => _effects[(int)EffectName.SkillAttackEffectEnd].SetActive(true);
    public void EnableAttackSmokeEffect() => _effects[(int)EffectName.AttackSmokeEffect].SetActive(true);
    public void EnableHeavySmokeEffect() => _effects[(int)EffectName.HeavyAttackEffect].SetActive(true);

}

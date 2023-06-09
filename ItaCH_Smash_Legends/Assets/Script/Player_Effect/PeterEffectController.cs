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
        JumpAttackEffect
    }
    public override void EnableFirstAttackEffect() => _effects[(int)EffectName.FirstAttackEffect].SetActive(true);
    public override void EnableSecondAttackFirstEffect() => _effects[(int)EffectName.SecondAttackFirstEffect].SetActive(true);
    public override void EnableSecondAttackSecondEffect() => _effects[(int)EffectName.SecondAttackSecondEffect].SetActive(true);
    public override void EnableFinishAttackEffect() => _effects[(int)EffectName.FinalAttackEffect].SetActive(true);
    public override void EnableHeavyAttackEffect() => _effects[(int)EffectName.HeavyAttackEffect].SetActive(true);
    public override void EnableJumpAttackEffect() => _effects[(int)EffectName.JumpAttackEffect].SetActive(true);
    public override void DisableJumpAttackEffect() => _effects[(int)EffectName.JumpAttackEffect].SetActive(false);


}

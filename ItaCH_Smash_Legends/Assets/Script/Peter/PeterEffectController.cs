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
    public void EnableFirstAttackEffect() => _effects[(int)EffectName.FirstAttackEffect].SetActive(true);
    public void EnableSecondAttackFirstEffect() => _effects[(int)EffectName.SecondAttackFirstEffect].SetActive(true);
    public void EnableSecondAttackSecondEffect() => _effects[(int)EffectName.SecondAttackSecondEffect].SetActive(true);
    public void EnableFinishAttackEffect() => _effects[(int)EffectName.FinalAttackEffect].SetActive(true);
    public void EnableHeavyAttackEffect() => _effects[(int)EffectName.HeavyAttackEffect].SetActive(true);
    public void EnableJumpAttackEffect() => _effects[(int)EffectName.JumpAttackEffect].SetActive(true);
    public void DisableJumpAttackEffect() => _effects[(int)EffectName.JumpAttackEffect].SetActive(false);


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliceEffectController : EffectController
{

    public enum EffectName
    {
        FirstAttackEffect,
        FirstAttackEndEffect,
        SecondAttackEffect,
        JumpAttackEffect,
        JumpSmokeEffect,
    }
    public void EnableFirstAttackEffect() => _effects[(int)EffectName.FirstAttackEffect].SetActive(true);
    public void EnableFirstAttackEndEffect() => _effects[(int)EffectName.FirstAttackEndEffect].SetActive(true);
    public void EnableSecondAttack() => _effects[(int)EffectName.SecondAttackEffect].SetActive(true);
    public void EnableJumpAttackEffect() => _effects[(int)EffectName.JumpAttackEffect].SetActive(true);
    public void EnableJumpSmokeEffect() => _effects[(int)EffectName.JumpSmokeEffect].SetActive(true);
    public void DisableJumpAttackEffect() => _effects[(int)EffectName.JumpAttackEffect].SetActive(false);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeterEffectController : EffectController
{
    public override void EnableFirstAttackEffect() => _effects[0].SetActive(true);
    public override void EnableSecondAttackFirstEffect() => _effects[1].SetActive(true);
    public override void EnableSecondAttackSecondEffect() => _effects[2].SetActive(true);
    public override void EnableFinishAttackEffect() => _effects[3].SetActive(true);
    public override void EnableSmashAttackEffect() => _effects[4].SetActive(true);
}

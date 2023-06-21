using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookSkillHit : HookHit
{
    void Start()
    {
        damage = skillDamage;
        knockbackUpDirection = skillKnockbackUpDirection;
        knockbackPower = lightKnockbackPower;
    }

}

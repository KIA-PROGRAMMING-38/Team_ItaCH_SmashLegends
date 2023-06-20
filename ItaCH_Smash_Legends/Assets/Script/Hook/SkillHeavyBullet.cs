using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHeavyBullet : HookBullet
{
   private void Awake()
    {
        bulletDeleteEffectPath = SKILL_BULLET_DELETE_EFFECT_PATH;
        bulletDeleteTime = SKILL_HEAVY_BULLET_DELETE_TIME;
        currentBulletSpeed = HEAVY_BULLET_SPEED;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHeavyBullet : HookBullet
{
    void Start()
    {
        bulletDeleteTime = SKILL_HEAVY_BULLET_DELETE_TIME;
        currentBulletSpeed = HEAVY_BULLET_SPEED;
    }

}

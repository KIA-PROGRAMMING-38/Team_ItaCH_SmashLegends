using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDefaultBullet : HookBullet
{
    void Start()
    {
        bulletDeleteTime = SKILL_BULLET_DELETE_TIME;
        currentBulletSpeed = DEFAULT_BULLET_SPEED;
    }
}

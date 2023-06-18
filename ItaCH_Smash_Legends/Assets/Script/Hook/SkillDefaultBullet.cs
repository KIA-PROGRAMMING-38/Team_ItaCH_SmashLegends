using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDefaultBullet : HookBullet
{
    void Start()
    {
        bulletDeleteTime = 0.28f;
        currentBulletSpeed = 20f;
    }
}

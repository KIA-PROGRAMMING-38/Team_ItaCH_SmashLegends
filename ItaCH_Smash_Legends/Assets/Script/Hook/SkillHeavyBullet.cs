using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHeavyBullet : HookBullet
{
    void Start()
    {
        bulletDeleteTime = 0.33f;
        currentBulletSpeed = 30f;
    }

}

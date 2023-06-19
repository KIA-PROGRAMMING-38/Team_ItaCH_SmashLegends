using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBullet : HookBullet
{
    private void Start()
    {
        bulletDeleteTime = HEAVY_BULLET_DELETE_TIME;
        currentBulletSpeed = HEAVY_BULLET_SPEED;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBullet : HookBullet
{
    private void Awake()
    {
        bulletDeleteEffectPath = HEAVY_BULLET_DELETE_EFFECT_PATH;
        bulletDeleteTime = HEAVY_BULLET_DELETE_TIME;
        currentBulletSpeed = HEAVY_BULLET_SPEED;
    }
}

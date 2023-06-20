using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastHeavyBullet : HookBullet
{
    private void Awake()
    {
        bulletDeleteEffectPath = LAST_HEAVY_BULLET_DELETE_EFFECT_PATH;
        bulletDeleteTime = HEAVY_BULLET_DELETE_TIME;
        currentBulletSpeed = HEAVY_BULLET_SPEED;
    }
}

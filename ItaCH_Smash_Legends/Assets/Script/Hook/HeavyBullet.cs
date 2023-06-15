using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBullet : HookBullet
{
    private void Start()
    {
        bulletDeleteTime = 0.3f;
        currentBulletSpeed = 30f;
    }
}

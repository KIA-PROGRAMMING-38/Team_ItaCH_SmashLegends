using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DefaultBullet : HookBullet
{
    private void Start()
    {
        bulletDeleteTime = 0.23f;
        currentBulletSpeed = 20f;
    }


}

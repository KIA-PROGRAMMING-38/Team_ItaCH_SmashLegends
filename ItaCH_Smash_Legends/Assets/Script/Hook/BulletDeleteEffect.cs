using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletDeleteEffect : MonoBehaviour
{
    public ObjectPool<BulletDeleteEffect> Pool { private get; set; }

    private void OnDisable()
    {
        Pool.Release(this);
    }
}

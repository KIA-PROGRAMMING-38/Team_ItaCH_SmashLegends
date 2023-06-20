using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FireEffect : MonoBehaviour
{
    public ObjectPool<FireEffect> pool { private get; set; }

    private void OnDisable()
    {
        pool.Release(this);
    }
}

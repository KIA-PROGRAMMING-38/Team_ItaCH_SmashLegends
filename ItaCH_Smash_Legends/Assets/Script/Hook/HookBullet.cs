using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookBullet : MonoBehaviour
{
    [SerializeField]
    private float _currentBulletSpeed = 30f;
    private float _elapsedTime;
    private float _bulletDeleteTime = 0.23f;
    public GameObject BulletDeleteEffect;

    private void Update()
    {
        transform.Translate(Vector3.forward * (_currentBulletSpeed * Time.deltaTime));
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= _bulletDeleteTime)
        {
            _elapsedTime = 0;
            GameObject effect = Instantiate(BulletDeleteEffect, transform.position, Quaternion.identity);
            effect.SetActive(true);
            gameObject.SetActive(false);
        }

    }


}

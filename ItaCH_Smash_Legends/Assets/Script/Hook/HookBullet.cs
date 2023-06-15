using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookBullet : MonoBehaviour
{
    protected float currentBulletSpeed = 20;
    protected float bulletDeleteTime = 0.23f;

    private float _elapsedTime;
    private GameObject _bulletDeleteEffect;

    private void Awake()
    {
        _bulletDeleteEffect = transform.GetChild(0).gameObject;
        _bulletDeleteEffect.SetActive(false);
    }

    private void Update()
    {
        BulletDirection();
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= bulletDeleteTime)
        {
            BulletPostProcessing();
        }
    }

    private void BulletDirection()
    {
        transform.Translate(Vector3.forward * (currentBulletSpeed * Time.deltaTime));
    }

    private  void BulletPostProcessing()
    {
        GameObject effect = Instantiate(_bulletDeleteEffect, transform.position, Quaternion.identity);
        effect.SetActive(true);
        _elapsedTime = 0;
        gameObject.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class PlayerHangController : MonoBehaviour
{
    private PlayerStatus _playerStatus;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private Collider _collider;
    
    public CancellationTokenSource TaskCancel;
    
    private float _hangPositionY = 2.5f;
    private float _fallingWaitTime = 3f;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _playerStatus = GetComponent<PlayerStatus>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HangZone") && _playerStatus.IsHang == false)
        {
            transform.forward = SetHangRotation(other.transform.position);
            transform.position = SetHangPosition(other);
            OnConstraints();

            _animator.Play(AnimationHash.Hang);
            _playerStatus.IsHang = true;
        }
    }

    private Vector3 SetHangRotation(Vector3 other)
    {
     Vector3 otherPosition = other.normalized;
        otherPosition.x = Mathf.Round(other.x);
        otherPosition.y = 0;
        otherPosition.z = Mathf.Round(other.z);
        
        return otherPosition * -1;
    }
    private Vector3 SetHangPosition(Collider other)
    {
        float[] hangPosition = new float[2];
        hangPosition[0] = other.transform.position.x;
        hangPosition[1] = other.transform.position.z;

        for (int i = 0; i < hangPosition.Length; ++i)
        {
            if (hangPosition[i] > 0)
            {
                hangPosition[i] -= 0.5f;
            }
            if (hangPosition[i] < 0)
            {
                hangPosition[i] += 0.5f;
            }
        }
        Vector3 setPosition = Vector3.zero;

        if (hangPosition[0] != 0)
        {
            setPosition = new Vector3(hangPosition[0], _hangPositionY, transform.position.z);
        }
        if (hangPosition[1] != 0)
        {
            setPosition = new Vector3(transform.position.x, _hangPositionY, hangPosition[1]);
        }

        return setPosition;

    }

    private void OnConstraints()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }
    
    public void OffConstraints()
    {
        _rigidbody.constraints = RigidbodyConstraints.None;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public async UniTaskVoid OnFalling()
    {
        TaskCancel = new();
        await UniTask.Delay(TimeSpan.FromSeconds(_fallingWaitTime), cancellationToken: TaskCancel.Token);
        _collider.isTrigger = true;
        _animator.Play(AnimationHash.HangFalling);


    }
}

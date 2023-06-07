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
    private float _hangPositionY = 0f;

    private float _fallingWaitTime = 3f;
    public CancellationTokenSource TaskCancel;

    private void Awake()
    {
        _collider= GetComponent<Collider>();
        _playerStatus = GetComponent<PlayerStatus>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HangZone") && _playerStatus.IsHang == false)
        {
            _playerStatus.IsHang = true;
            _animator.Play(AnimationHash.Hang);

            transform.forward = SetHangRotation(other.transform.position);
            transform.position = SetHangPosition(other);
        }
    }

    private Vector3 SetHangRotation(Vector3 other)
    {
        other = other.normalized * - 1;
        other.x = Mathf.Round(other.x);
        other.y = Mathf.Round(other.y);
        other.z = Mathf.Round(other.z);

        return other;
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
    // ������Ʈ �ӽ� -> �� ������Ʈ ���� Ż���Ҷ� ���
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHangController : MonoBehaviour
{
    private PlayerStatus _playerStatus;

    private Rigidbody _rigidbody;
    private Animator _animator;

    private void Awake()
    {
        _playerStatus = GetComponent<PlayerStatus>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HangZone"))
        {
            OnConstraints();
            _playerStatus.IsHang = true;
            _animator.Play(AnimationHash.Hang);

            transform.forward = (other.transform.position.normalized * -1);
            transform.position = SetHangPosition(other);
        }
    }

    private Vector3 SetHangPosition(Collider other)
    {
        float[] hangPosition = new float[3];
        hangPosition[0] = other.transform.position.x;
        hangPosition[1] = other.transform.position.y;
        hangPosition[2] = other.transform.position.z;

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
            setPosition = new Vector3(hangPosition[0], hangPosition[1], transform.position.z);
        }
        if (hangPosition[2] != 0)
        {
            setPosition = new Vector3(transform.position.x, hangPosition[1], hangPosition[2]);
        }

        return setPosition;

    }

    private void OnConstraints()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }
    // 스테이트 머신 -> 행 스테이트 에서 탈출할때 사용
    public void OffConstraints()
    {
        _rigidbody.constraints = RigidbodyConstraints.None;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

}

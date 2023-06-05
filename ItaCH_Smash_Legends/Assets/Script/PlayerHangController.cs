using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHangController : MonoBehaviour
{
    private PlayerStatus _playerStatus;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private float _hangPositionY = 0.5f;

    public IEnumerator WaitFallingCoroutine;

    private WaitForSeconds _waitForFallingSecond = new WaitForSeconds(3);

    private void Awake()
    {
        _playerStatus = GetComponent<PlayerStatus>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        WaitFallingCoroutine = OnFalling();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HangZone") && _playerStatus.IsHang == false)
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
    // 스테이트 머신 -> 행 스테이트 에서 탈출할때 사용
    public void OffConstraints()
    {
        _rigidbody.constraints = RigidbodyConstraints.None;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public IEnumerator OnFalling()
    {
        while (true)
        {
            yield return _waitForFallingSecond;
            Debug.Log("떨어진다");


            break;
        }



    }
}

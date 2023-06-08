using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollUp : MonoBehaviour
{
    private PlayerMove _playerMove;
    private PlayerStatus _playerStatus;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private float _rollingDashPower = 1.2f;

    public Vector3 RollingForward;

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerStatus = GetComponent<PlayerStatus>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void RollingDash(Vector3 position)
    {
        _rigidbody.AddForce(position * _rollingDashPower, ForceMode.Impulse);
        _playerStatus.IsRollUp = false;
    }

    public void RollingDirection()
    {
        if (_playerMove.moveDirection != Vector3.zero)
        {
            if (RollingForward == _playerMove.moveDirection)
            {
                transform.forward = -1 * _playerMove.moveDirection;
                _animator.SetTrigger(AnimationHash.RollUpBack);
                return;
            }

            else if (_playerMove.moveDirection.x != 0 && _playerMove.moveDirection.z != 0)
            {
                SetDiagonalRolling();
                return;
            }

            else
            {
                _animator.SetTrigger(AnimationHash.RollUpFront);
                transform.forward = _playerMove.moveDirection;
                return;
            }
        }
    }

    // DownIdle 상태에서 호출 
    private async UniTaskVoid RoiingInputWaitTask()
    {
        while (_playerStatus.IsRollUp)
        {
            await UniTask.Delay(100);
            if (_playerMove.moveDirection != Vector3.zero)
            {
                RollingDash(_playerMove.moveDirection);
            }

        }
    }

    private void SetDiagonalRolling()
    {
        if (RollingForward.x > 0)
        {
            if (_playerMove.moveDirection.x > 0)
            {
                transform.forward = -1 * _playerMove.moveDirection;
                _animator.SetTrigger(AnimationHash.RollUpBack);
            }
            else
            {
                transform.forward = _playerMove.moveDirection;
                _animator.SetTrigger(AnimationHash.RollUpFront);
            }
        }
        else
        {
            if (_playerMove.moveDirection.x < 0)
            {
                transform.forward = -1 * _playerMove.moveDirection;
                _animator.SetTrigger(AnimationHash.RollUpBack);
            }
            else
            {
                transform.forward = _playerMove.moveDirection;
                _animator.SetTrigger(AnimationHash.RollUpFront);
            }
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        //플레이어 피격시 체공 후 가 조건
        if (other.CompareTag("Finish"))
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            Animator _anim = GetComponent<Animator>();

            rigidbody.AddForce(-transform.forward + transform.up, ForceMode.Impulse);
            RollingForward = -transform.forward;


            _anim.Play(AnimationHash.DownIdle);
            _playerStatus.IsRollUp = true;
            RoiingInputWaitTask().Forget();

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _currentMoveSpeed;

    private Vector3 _moveDirection;

    void Update()
    {
        MoveAndRotate();
    }

    // 이동방향을 바라보도록 이동방향으로 캐릭터 회전
    private void MoveAndRotate()
    {
        if (_moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(_moveDirection);
            transform.Translate(Vector3.forward * (_currentMoveSpeed * Time.deltaTime));
        }
    }

    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        if (input != null)
        {
            _moveDirection = new Vector3(input.x, 0, input.y);
        }
    }
}

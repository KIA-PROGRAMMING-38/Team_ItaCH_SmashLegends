using UnityEngine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;

public class PlayerAttack : MonoBehaviour
{

    public readonly int MAX_POSSIBLE_ATTACK_COUNT = 3;
    public readonly int COMBO_SECOND_COUNT = 2;
    public readonly int COMBO_FINISH_COUNT = 1;
    public int CurrentPossibleComboCount;

    internal bool isFirstAttack;
    internal bool isSecondAttack;
    internal bool isFinishAttack;

    private PlayerMove _playerMove;
    private Rigidbody _rigidbody;
    public Collider attackRange;

    private float _defaultDashPower = 1f;

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        _rigidbody = GetComponent<Rigidbody>();
        CurrentPossibleComboCount = MAX_POSSIBLE_ATTACK_COUNT;
    }

    public void AttackOnDefaultDash()
    {
        _rigidbody.AddForce(transform.forward * _defaultDashPower, ForceMode.Impulse);
    }
    public void AttackRotate()
    {
        if (_playerMove.moveDirection != Vector3.zero)
        {
            transform.forward = _playerMove.moveDirection;
        }
    }
   
}

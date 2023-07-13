using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    // 회의 후 생존 및 삭제 여부 결정
    private EffectController _effectController;
    protected Animator _animator;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _effectController = GetComponent<EffectController>();
    }

    
}
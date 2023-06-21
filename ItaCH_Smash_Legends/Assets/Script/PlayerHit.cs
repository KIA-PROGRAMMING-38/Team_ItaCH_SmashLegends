using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour, IHit
{
    protected float lightKnockbackPower;
    protected float heavyKnockbackPower;
    internal bool invincible;

    protected Vector3 _knockbackDirection;

    protected CharacterStatus _characterStatus;
    protected PlayerStatus _playerStatus;
    protected Animator _animator;

    protected void Start()
    {
        _characterStatus = GetComponent<CharacterStatus>();
        _playerStatus = GetComponent<PlayerStatus>();
        _animator = GetComponent<Animator>();

        // TODO : ������ �ذ�Ǹ� ���� ó��
        //invincible = true;
    }
    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && _playerStatus.CurrentState == PlayerStatus.State.HitUp)
        {
            _animator.SetTrigger(AnimationHash.HitDown);
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !invincible)
        {
            other.transform.forward = (-1) * transform.forward;
            Hit(other);
        }
    }
    public virtual void Hit(Collider other)
    {

    }
    
}

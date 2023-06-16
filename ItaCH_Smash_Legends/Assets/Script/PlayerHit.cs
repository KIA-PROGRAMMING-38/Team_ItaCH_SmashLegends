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

        invincible = false;
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
        if (other.CompareTag("Player"))
        {
            Vector3 hitPoint = other.transform.position - transform.position;
            other.transform.rotation = Quaternion.LookRotation(-hitPoint);
            Hit(other);
        }
    }
    public virtual void Hit(Collider other)
    {

    }
    public void GetHit(float power, int animationHash, Collider other /*int damage*/)
    {
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        Animator animator = other.GetComponent<Animator>();
        //CharacterStatus opponentCharacter = GetComponent<CharacterStatus>();

        rigidbody.AddForce(_knockbackDirection * power, ForceMode.Impulse);
        animator.SetTrigger(animationHash);
        //opponentCharacter.GetDamage(damage);
    }
}

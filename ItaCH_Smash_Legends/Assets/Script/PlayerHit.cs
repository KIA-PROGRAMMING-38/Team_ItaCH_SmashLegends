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

    private void Start()
    {
        _characterStatus = GetComponent<CharacterStatus>();
        _playerStatus = GetComponent<PlayerStatus>();
        _animator = GetComponent<Animator>();

        // TODO : 무적이 해결되면 같이 처리
        //invincible = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && _playerStatus.CurrentState == PlayerStatus.State.HitUp)
        {
            _animator.SetTrigger(AnimationHash.HitDown);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.forward = (-1) * transform.forward;
            Hit(other);
        }
    }
    public virtual void Hit(Collider other)
    {

    }
}
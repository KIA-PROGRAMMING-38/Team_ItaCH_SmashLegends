using UnityEngine;

public class PlayerHit : MonoBehaviour, IHit
{
    private EffectController _effectController;
    private PlayerHit _playerHit;
    protected CharacterStatus _characterStatus;
    protected PlayerStatus _playerStatus;
    protected Animator _animator;

    protected float defaultKnockbackPower;
    protected float heavyKnockbackPower;
    internal bool invincible;

    protected Vector3 _knockbackDirection;

    private void Start()
    {
        _characterStatus = GetComponent<CharacterStatus>();
        _playerStatus = GetComponent<PlayerStatus>();
        _animator = GetComponent<Animator>();

        defaultKnockbackPower = _characterStatus.DefaultKnockbackPower;
        heavyKnockbackPower = _characterStatus.HeavyKnockbackPower;
        invincible = false;
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
            _playerHit = other.GetComponent<PlayerHit>();
            _effectController = other.GetComponent<EffectController>();

            if (_playerHit.invincible == false)
            {
                other.transform.forward = (-1) * transform.forward;
                _effectController.StartHitFlashEffet().Forget();
                Hit(other);
            }
        }
    }
    public virtual void Hit(Collider other)
    {

    }
}
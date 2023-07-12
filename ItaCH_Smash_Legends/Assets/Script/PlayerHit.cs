using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    private EffectController _effectController;
    protected PlayerStatus _playerStatus;
    protected Animator _animator;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && _playerStatus.CurrentState == PlayerStatus.State.HitUp)
        {
            _animator.SetTrigger(AnimationHash.HitDown);
        }
    }

    public void GetKnockbackOnAttack(Collider other, int animationHash, KnockbackType type)
    {
        transform.forward = -1 * other.transform.forward;
        Vector3 _knockbackDirection = other.transform.forward + transform.up;

        _animator.SetTrigger(animationHash);
        _rigidbody.AddForce(_knockbackDirection * SetKnockbackPower(type, other), ForceMode.Impulse);
        _effectController.StartHitFlashEffet().Forget();
    }
    private float SetKnockbackPower(KnockbackType type, Collider other)
    {
        CharacterStatus otherStatus = other.GetComponent<CharacterStatus>();
        float knockbackPower = 0;

        switch (type)
        {
            case KnockbackType.Default:
                knockbackPower = otherStatus.Stat.DefaultKnockbackPower;
                break;

            case KnockbackType.Heavy:
                knockbackPower = otherStatus.Stat.HeavyKnockbackPower;
                break;
        }

        return knockbackPower;
    }

    // 기존 로직 => 추후 삭제
    public void LengendHit(Collider other)
    {
        Vector3 _knockbackDirection = transform.forward + transform.up;
        float power = 0.3f;
        float heavyPower = 0.5f;
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        LegendAnimationController animator = other.GetComponent<LegendAnimationController>();

        //animator.TriggerAnimation(AnimationHash.Hit);
        rigidbody.AddForce(_knockbackDirection * power, ForceMode.Impulse);

        //if (_legendAnimationController._isHitUp)
        //{
        //    animator.TriggerAnimation(AnimationHash.HitUp);
        //    rigidbody.AddForce(_knockbackDirection * heavyPower, ForceMode.Impulse);
        //}
    }

}
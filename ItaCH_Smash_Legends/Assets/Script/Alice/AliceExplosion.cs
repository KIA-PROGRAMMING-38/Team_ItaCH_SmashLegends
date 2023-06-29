using UnityEngine;

public class AliceExplosion : MonoBehaviour
{
    private PlayerHit _playerHit;
    void Start()
    {
        _playerHit= GetComponent<PlayerHit>();
    }

    private void OnParticleCollision(GameObject other)
    {
        float heavyKnockbackPower = 0.8f;

        AliceGetHit(heavyKnockbackPower, AnimationHash.Hit, other);
    }

    private void AliceGetHit(float power, int animationHash, GameObject other)
    {
        Vector3 knockbackDirection = transform.up;
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        Animator animator = other.GetComponent<Animator>();
        //CharacterStatus opponentCharacter = GetComponent<CharacterStatus>();

        rigidbody.AddForce(knockbackDirection * power, ForceMode.Impulse);
        animator.SetTrigger(animationHash);
        //opponentCharacter.GetDamage(damage); // 피격 판정 수정 필요 
    }
}

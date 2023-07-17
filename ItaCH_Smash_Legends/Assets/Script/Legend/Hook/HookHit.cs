using Photon.Pun;
using UnityEngine;

public class HookHit : MonoBehaviour
{
    protected float defaultKnockbackPower = 0.05f;
    protected float heavyKnockbackPower = 0.7f;
    protected float knockbackPower;
    protected int animationHashValue = AnimationHash.Hit;
    protected int defaultdamage = 100;
    protected int skillDamage = 50;
    protected int heavyDamage = 200;
    protected Vector3 heavyKnockbackUpDirection = new Vector3(0, 0.7f, 0);
    protected Vector3 skillKnockbackUpDirection = new Vector3(0, 0.15f, 0);
    protected Vector3 knockbackUpDirection = new Vector3(0, 0.3f, 0);

    private Vector3 _knockbackDirection;
    private Vector3 _bulletHitPosition;
    private HookBullet _hookBullet;
    private EffectController _effectController;
    private LegendController _legendController;

    private void Start()
    {
        _hookBullet = GetComponentInParent<HookBullet>();
        _legendController = _hookBullet.constructor.GetComponent<LegendController>();
        SetPowerAndDamage();
        knockbackPower = defaultKnockbackPower;
        CalculationPowerAndDamage();
    }
    protected virtual void CalculationPowerAndDamage()
    {

    }
    private void SetPowerAndDamage()
    {
        defaultKnockbackPower = _legendController.Stat.DefaultKnockbackPower;
        heavyKnockbackPower = _legendController.Stat.HeavyKnockbackPower;
        defaultdamage = _legendController.Stat.DefaultAttackDamage;
        skillDamage = _legendController.Stat.SkillAttackDamage;
        heavyDamage = _legendController.Stat.SkillAttackDamage;
    }
    private void OnTriggerEnter(Collider other)
    {
        // Hit 리펙토링 후 수정 스크립트

        //if (other.CompareTag("Player") && other.gameObject.layer != _hookBullet.constructor.layer)
        //{
        //    PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
        //    _playerHit = other.GetComponent<PlayerHit>();
        //    if (_playerHit.invincible == false)
        //    {
        //        _effectController = other.GetComponent<EffectController>();
        //        SetDirection(other);

        //        if (PossibleAttack(playerStatus))
        //        {
        //            GetBulletKnockBack(other);
        //            _effectController.StartHitFlashEffet().Forget();
        //        }
        //        GetHitDamage(other);
        //        _hookBullet.BulletPostProcessing(BulletDeleteEffectPosition(other));
        //    }
        //}
    }

    //private bool PossibleAttack(PlayerStatus playerStatus)
    //{
    //    if (playerStatus.CurrentState != PlayerStatus.State.HitUp && playerStatus.CurrentState != PlayerStatus.State.SkillAttack)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
    private void SetDirection(Collider other)
    {
        _knockbackDirection = knockbackPower * _hookBullet.constructor.transform.forward;
        other.transform.forward = -1 * _hookBullet.constructor.transform.forward;
    }
    private void GetBulletKnockBack(Collider other)
    {
        Animator animator = other.GetComponent<Animator>();
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();

        if (rigidbody.velocity != Vector3.zero)
        {
            rigidbody.velocity = Vector3.zero;
        }
        rigidbody.AddForce(_knockbackDirection + knockbackUpDirection, ForceMode.Impulse);
        animator.Play(animationHashValue);
    }

    private Vector3 BulletDeleteEffectPosition(Collider other)
    {
        _bulletHitPosition.x = other.transform.position.x;
        _bulletHitPosition.y = transform.position.y;
        _bulletHitPosition.z = other.transform.position.z;

        return _bulletHitPosition;
    }

    private void GetHitDamage(Collider other) // To Do : @gitLeejw와 논의 필요
    {
        LegendController opponentCharacter = other.GetComponent<LegendController>();
        opponentCharacter.Damage(defaultdamage);
    }
}

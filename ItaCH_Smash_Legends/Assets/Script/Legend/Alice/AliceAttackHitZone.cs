using UnityEngine;

public class AliceAttackHitZone : HitZone
{
    void Start()
    {
        // TODO : 스탯 연동후 재설정 
        damageAmount = 100;
        knockbackPower = 0.3f;
        animationType = AnimationHash.Hit;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        knockbackDirection = transform.forward + transform.up;
    }
}

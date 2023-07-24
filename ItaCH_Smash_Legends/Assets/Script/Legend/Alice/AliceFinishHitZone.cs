using UnityEngine;

public class AliceFinishHitZone : HitZone
{
    void Start()
    {
        // TODO : 스탯 연동후 재설정 
        damageAmount = 100;
        knockbackPower = 1f;
        animationType = AnimationHash.HitUp;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        knockbackDirection = transform.forward + transform.up;
    }
}

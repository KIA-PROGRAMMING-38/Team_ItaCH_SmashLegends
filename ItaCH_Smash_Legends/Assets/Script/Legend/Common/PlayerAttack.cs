using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    protected CharacterStatus characterStatus;
    protected Rigidbody attackRigidbody;
    private void Awake()
    {
        characterStatus= GetComponent<CharacterStatus>();
        attackRigidbody= GetComponent<Rigidbody>();
    }
    public virtual void DashOnAnimationEvent()
    {

    }
}

using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        CharacterStatus playerCharacter = other.gameObject.GetComponent<CharacterStatus>();
        if (!playerCharacter)
        {
            return;
        }
        else
        {
            playerCharacter._isDead = true;
            playerCharacter.gameObject.SetActive(false);
            playerCharacter.GetDamage(int.MaxValue);
        }
    }
}

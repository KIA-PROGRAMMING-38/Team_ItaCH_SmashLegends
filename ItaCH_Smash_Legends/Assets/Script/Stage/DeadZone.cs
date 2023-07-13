using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        LegendController playerCharacter = other.gameObject.GetComponent<LegendController>();
        if (!playerCharacter)
        {
            return;
        }
        else
        {
            //playerCharacter._isDead = true; // TO DO : 사망 판정 로직 리팩토링 이후 이식
            playerCharacter.gameObject.SetActive(false);
            playerCharacter.Damage(int.MaxValue);
        }
    }
}

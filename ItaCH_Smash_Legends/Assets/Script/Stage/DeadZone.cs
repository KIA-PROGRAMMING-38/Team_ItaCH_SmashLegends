using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        var legend = other.GetComponent<LegendController>();
        legend.SetDieEffect();
        Managers.SoundManager.Play(SoundType.SFX, StringLiteral.SFX_LEGEND_DEAD);
        legend.gameObject.SetActive(false);
    }
}

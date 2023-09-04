using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        var legend = other.GetComponent<LegendController>();
        if(legend.Stat.HP > 0)
        {
            legend.Damage(int.MaxValue);
        }
        Managers.SoundManager.Play(SoundType.Voice, legend: legend.LegendType, voice: VoiceType.Die);
        Managers.SoundManager.Play(SoundType.SFX, StringLiteral.SFX_LEGEND_DEAD);
        legend.SetDieEffect();
        legend.gameObject.SetActive(false);
    }
}

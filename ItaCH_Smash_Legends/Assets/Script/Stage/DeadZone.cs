using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        var legend = other.GetComponent<LegendController>();
        if (legend.Stat.HP > 0)
        {
            legend.Damage(int.MaxValue);
        }
        legend.SetDieEffect();
        legend.gameObject.SetActive(false);
    }
}

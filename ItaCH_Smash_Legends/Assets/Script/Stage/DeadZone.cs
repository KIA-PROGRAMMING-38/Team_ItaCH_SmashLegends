using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        var legend = other.GetComponent<LegendController>();
        legend.gameObject.SetActive(false);
    }
}

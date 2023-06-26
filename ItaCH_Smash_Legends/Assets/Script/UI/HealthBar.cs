using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image _filling;

    public void InitHealthBarSettings()
    {
        _filling = transform.GetChild(0).GetComponent<Image>();
    }

    public void SetHealthPoint(int healthPoint, int healthPointPercent)
    {
        float healthPointRatio = healthPointPercent * 0.01f;
        _filling.fillAmount = healthPointRatio;
    }
}

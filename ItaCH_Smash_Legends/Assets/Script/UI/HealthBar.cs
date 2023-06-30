using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Util.Method;

public class HealthBar : MonoBehaviour
{
    private Image _filling;

    public void InitHealthBarSettings()
    {
        _filling = transform.GetChild(0).GetComponent<Image>();
    }

    public void SetHealthPoint(int healthPoint, int healthPointPercent)
    {
        if(healthPointPercent.Equals(100f))
        {
            Method.ChangeFillAmountGradually(0, 1, _filling).Forget();
        }
        float healthPointRatio = healthPointPercent * 0.01f;
        _filling.fillAmount = healthPointRatio;
    }
}

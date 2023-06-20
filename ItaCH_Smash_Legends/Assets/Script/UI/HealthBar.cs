using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private CharacterStatus _characterStatus;
    private Image _filling;

    public void InitHealthBarSettings(CharacterStatus characterStatus)
    {
        _characterStatus = characterStatus;
        _filling= transform.GetChild(0).GetComponent<Image>();
        _characterStatus.OnPlayerHealthPointChange -= SetHealthPoint;
        _characterStatus.OnPlayerHealthPointChange += SetHealthPoint;
    }

    public void SetHealthPoint(int healthPoint, int healthPointPercent)
    {
        float healthPointRatio = healthPointPercent * 0.01f;
        _filling.fillAmount = healthPointRatio;
    }

    private void OnDestroy()
    {
        _characterStatus.OnPlayerHealthPointChange -= SetHealthPoint;
    }
}

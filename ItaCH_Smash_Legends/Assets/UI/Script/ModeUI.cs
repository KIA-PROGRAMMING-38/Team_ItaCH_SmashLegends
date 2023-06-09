using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeUI : MonoBehaviour
{
    [SerializeField] private HealthBar[] _healthPointBars;

    public void InitModeUISettings(GameObject[] players)
    {
        for (int i = 0; i < players.Length - 1; ++i)
        {
            _healthPointBars[i].InitHealthBarSettings(players[i + 1].GetComponent<CharacterStatus>());
        }
    }
}

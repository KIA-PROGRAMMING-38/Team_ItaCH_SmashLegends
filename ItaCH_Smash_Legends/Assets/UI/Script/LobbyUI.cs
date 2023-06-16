using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    void Start()
    {
        SoundManager._instance.Play("Lobby", SoundType.BGM);
    }
}

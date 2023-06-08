using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameMode
{
    private GameModeType _currentGameModeType;
    private GameObject _currentMap;
    public GameModeType GameModeType { get => _currentGameModeType; set => _currentGameModeType = value; }
    public GameObject Map { get => _currentMap; private set => _currentMap = value; }
}

using UnityEngine;

public class CharacterDefaultStatus : MonoBehaviour
{
    // Data 파일에서 파싱될 고정 데이터
    private const int DATA_VALUE = 10;
    public int MaxHealthPoint { get => _maxHealthPointTest; private set => _maxHealthPointTest = value; } 
    private int _maxHealthPointTest = DATA_VALUE;
}

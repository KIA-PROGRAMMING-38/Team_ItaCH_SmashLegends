using UnityEngine;

public class CharacterDefaultStatus : MonoBehaviour
{
    // Data ���Ͽ��� �Ľ̵� ���� ������
    private const int DATA_VALUE = 10;
    public int MaxHealthPoint { get => _maxHealthPointTest; private set => _maxHealthPointTest = value; } 
    private int _maxHealthPointTest = DATA_VALUE;
}

using UnityEngine;

public class CharacterDefaultStatus : MonoBehaviour
{
    // Data ���Ͽ��� �Ľ̵� ���� ������
    private const int DATA_VALUE = 4000;
    public int MaxHealthPoint { get => _maxHealthPointTest; private set => _maxHealthPointTest = value; } 
    private int _maxHealthPointTest = DATA_VALUE;  
}

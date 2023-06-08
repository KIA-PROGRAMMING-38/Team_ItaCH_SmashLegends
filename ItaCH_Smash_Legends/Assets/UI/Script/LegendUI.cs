using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LegendUI : MonoBehaviour
{
    [SerializeField] private float _heightOffset;
    private Transform _playerTransform;
    private CharacterStatus _playerStatus;
    private float _healthPointBarFillAmount;
    private float _healthPointBarScale;
    private string _healthAmountText;

    void Update()
    {
        transform.position = new Vector3(_playerTransform.position.x, 
            _playerTransform.position.y + _heightOffset,
            _playerTransform.position.z);
    }

    public void SetTransform(Transform playerTransform)
    {
        _playerTransform = playerTransform;
        CharacterStatus characterStatus = playerTransform.GetComponent<CharacterStatus>();
        if (characterStatus == null)
        {
            characterStatus = playerTransform.AddComponent<CharacterStatus>();
        }
        _playerStatus = characterStatus;
    }
    public void SetHealthPointBar(int HealthPoint)
    {
        
    }

}

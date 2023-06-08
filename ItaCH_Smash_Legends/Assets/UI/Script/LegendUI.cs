using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendUI : MonoBehaviour
{
    [SerializeField] private float _heightOffset;
    private Transform _playerTransform;
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
    }
    public void SetHealthPointBar(int HealthPoint)
    {
        
    }

}

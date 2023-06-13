using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScaler : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Vector2 _originalButtonScale;
    [SerializeField] private Vector2 _targetButtonScale;
    void Start()
    {
        if(_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalButtonScale = _rectTransform.localScale;
        }
    }

    public void OnPressButton()
    {
        _originalButtonScale = _targetButtonScale;
    }
}

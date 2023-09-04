using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingJoyStick : MonoBehaviour
{
    public RectTransform rectTransform;
    public RectTransform Knob;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
}

using System;
using Unity.VisualScripting;
using UnityEngine;

public static class Extensions
{
    public static void FlipY(this RectTransform rectTransform, Space RelativeTo = Space.Self)
    {
        rectTransform.Rotate(0, 180, 0, RelativeTo);
    }

    public static void BindEvent(this GameObject go, Action action, UIEventType type = UIEventType.Click)
    {
        UIBase.BindEvent(go, action, type);
    }
}
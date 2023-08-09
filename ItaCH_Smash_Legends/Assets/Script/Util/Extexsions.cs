using UnityEngine;

public static class Extensions
{
    public static void FlipY(this RectTransform rectTransform, Space RelativeTo = Space.Self)
    {
        rectTransform.Rotate(0, 180, 0, RelativeTo);
    }
}
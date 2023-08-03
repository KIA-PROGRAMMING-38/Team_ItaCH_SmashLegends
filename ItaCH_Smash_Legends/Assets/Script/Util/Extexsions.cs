using UnityEngine;

public static class Extensions
{
    public static void FlipY(this RectTransform rectTransform, bool isFlipped = true)
    {
        if (false == isFlipped)
        {
            return;
        }

        rectTransform.Rotate(0, 180, 0);
    }

    public static bool IsRedTeam(this TeamType teamType)
    {
        if (teamType == TeamType.Red)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}
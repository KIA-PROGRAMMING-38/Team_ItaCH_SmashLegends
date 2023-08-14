using UnityEngine;

public class Define
{
    public static readonly Color32 DEFAULT_UI_PORTRAIT_COLOR = new Color32(0, 0, 0, 0);
    public static readonly Color32 BLUE_TEAM_UI_PORTRAIT_COLOR = new Color32(131, 187, 253, 255);
    public static readonly Color32 RED_TEAM_UI_PORTRAIT_COLOR = new Color32(241, 140, 134, 255);

    public static readonly Color[] UI_PORTRAIT_COLORS =
    {
        DEFAULT_UI_PORTRAIT_COLOR,
        BLUE_TEAM_UI_PORTRAIT_COLOR,
        RED_TEAM_UI_PORTRAIT_COLOR
    };

    public static readonly Color32 DEFAULT_DAMAGE_BUFFER_COLOR = DEFAULT_UI_PORTRAIT_COLOR;
    public static readonly Color32 BLUE_TEAM_DAMAGE_BUFFER_COLOR = new Color32(155, 255, 0, 255);
    public static readonly Color32 RED_TEAM_DAMAGE_BUFFER_COLOR = new Color32(255, 255, 60, 255);


    public static readonly Color[] DAMAGE_BUFFER_COLORS =
    {
        DEFAULT_DAMAGE_BUFFER_COLOR,
        BLUE_TEAM_DAMAGE_BUFFER_COLOR,
        RED_TEAM_DAMAGE_BUFFER_COLOR
    };
}
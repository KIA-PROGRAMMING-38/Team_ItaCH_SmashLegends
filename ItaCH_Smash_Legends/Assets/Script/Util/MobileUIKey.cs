using System.Collections.Generic;

namespace Util
{
    public static class MobileUIKey
    {
        public const string Id = "id";
        public const string ResourceType = "resource_type";
        public const string ResourceName = "resource_name";
        public const string PositionX = "pos_x";
        public const string PositionY = "pos_y";
        public const string PositionZ = "pos_z";
        public const string Width = "width";
        public const string Height = "height";
        public const string AnchorMinX = "anchor_min_x";
        public const string AnchorMinY = "anchor_min_y";
        public const string AnchorMaxX = "anchor_max_x";
        public const string AnchorMaxY = "anchor_max_y";
        public const string PivotX = "pivot_x";
        public const string PivotY = "pivot_y";
        public const string RotationX = "rotation_y";
        public const string RotationY = "rotation_x";
        public const string RotationZ = "rotation_z";
        public const string ScaleX = "scale_x";
        public const string ScaleY = "scale_y";
        public const string ScaleZ = "scale_z";

        public static void SwapData(List<Dictionary<string, object>> mobileData,
            MobileUIName beforeName, string beforeKey,
            MobileUIName afterName, string afterKey)
        {
            mobileData[(int)beforeName][beforeKey] = mobileData[(int)afterName][afterKey];
        }
    }

    public enum MobileUIName
    {
        AttackIconOutline = 0,
        AliceDefaultAttack,
        HookDefaultAttack,
        PeterDefaultAttack,
        AttackIconShadow,
        HeavyAttackIconOutline,
        AliceHeavyAttack,
        HookHeavyAttack,
        PeterHeavyAttack,
        HeavyAttackIconShadow,
        SkillAttackOutline,
        AliceSkillAttack,
        HookSkillAttack,
        PeterSkillAttack,
        SkillAttackIconShadow,
        JumpOutline,
        JumpIcon,
        JumpIconShadow,
        MovePad,
        JoyStick,
        JoyStickOutline,
        MovePadShadow
    }

}

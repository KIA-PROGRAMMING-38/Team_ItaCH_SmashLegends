using System.Text;

namespace Util.Path
{
    public static class FilePath
    {
        // TO DO : 개별 기능 리팩토링 시 해당 부분에서 관리
        #region Folder Path
        public const string UIResources = "UI/";
        public const string UISpritePath = "UI/Sprite/";
        public const string AliceIcon = "UI/MobileUI/Sprite/Alice/Alice";
        public const string HookIcon = "UI/MobileUI/Sprite/Hook/Hook";
        public const string PeterIcon = "UI/MobileUI/Sprite/Peter/Peter";
        #endregion

        #region UI File Path
        public const string LegendJumpIcon = "UI/MobileUI/Sprite/Character_Jump_Icon";
        public const string AliceDefaultAttackIcon = "UI/MobileUI/Sprite/Alice/Alice_DefaultAttackIcon";
        public const string AliceHeavyAttackIcon = "UI/MobileUI/Sprite/Alice/Alice_HeavyAttackIcon";
        public const string AliceSkillAttackIcon = "UI/MobileUI/Sprite/Alice/Alice_SkillAttackIcon";
        public const string HookDafaultAttackIcon = "UI/MobileUI/Sprite/Hook/Hook_DefaultAttackIcon";
        public const string HookHeavyAttackIcon = "UI/MobileUI/Sprite/Hook/Hook_HeavyAttackIcon";
        public const string HookSkillAttackIcon = "UI/MobileUI/Sprite/Hook/Hook_SkillAttackIcon";
        public const string PeterDefaultAttackIcon = "UI/MobileUI/Sprite/Peter/Peter_DefaultAttackIcon";
        public const string PeterHeavyAttackIcon = "UI/MobileUI/Sprite/Peter/Peter_HeavyAttackIcon";
        public const string PeterSkillAttackIcon = "UI/MobileUI/Sprite/Peter/Peter_SkillAttackIcon";
        public const string AttackOutline = "UI/MobileUI/Sprite/TouchController/Attack_Outline";
        public const string MovePad = "UI/MobileUI/Sprite/TouchController/Move_Pad";
        public const string MovePadOutline = "UI/MobileUI/Sprite/TouchController/Move_Pad_Outline";
        public const string JoyStick = "UI/MobileUI/Sprite/TouchController/JoyStick";
        public const string JoyStickOutline = "UI/MobileUI/Sprite/TouchController/JoyStick_Outline";
        public const string SecondaryOutline = "UI/MobileUI/Sprite/TouchController/Secondary_Outline";
        public const string Shadow = "UI/MobileUI/Sprite/TouchController/Shadow";
        #endregion

        #region Prefabs Path
        public const string LegendMenuUIPath = "UI/LegendMenu";
        public const string LegendMenuButtonPath = "UI/LegendMenuButton";
        public const string SettingUIPath = "UI/SettingPanel";
        public const string SettingButtonPath = "UI/SettingButton";
        public const string MatchingUIPath = "UI/MatchingPanel";
        public const string MatchingButtonPath = "UI/GameStartButton";
        public const string LobbyWorldPrefabPath = "Map/LobbyWorld/LobbySceneWorld";
        public const string MapPath = "Map/LobbyWorld/";
        public const string HeavyBulletDeleteEffect = "Prefab/Hook/Hook_Ingame/Hook_Heavy_Bullet_Delete_Effect";
        public const string LastHeavyBulletDeleteEffect = "Prefab/Hook/Hook_Ingame/Hook_Last_Heavy_Bullet_Delete_Effect";
        public const string SkillBulletDeleteEffect = "Prefab/Hook/Hook_Ingame/Hook_SKill_Bullet_Delete_Effect";
        public const string BulletDeleteEffect = "Prefab/Hook/Hook_Ingame/Hook_Default_Bullet_Delete_Effect";
        public const string HookAnimator = "Hook";
        #endregion

        #region CSV File Path        
        public const string MobileUIDataBase = "Data/UI_Data/Mobile_UI_DataBase";
        #endregion


        // TO DO : UI 리팩토링 시 완전히 ResourceManager로 넘길 부분, StringBuilder 생성부 삭제
        
        public static string GetLegendSpritePath(LegendType legendType)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string legendName = legendType.ToString();
            stringBuilder.Append(UISpritePath);
            stringBuilder.Append(legendName);
            stringBuilder.Append($"/{legendName}");
            return stringBuilder.ToString();
        }
    }
}
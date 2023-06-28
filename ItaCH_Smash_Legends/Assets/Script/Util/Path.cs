using System.Text;
using Util.Enum;

namespace Util.Path
{
    public static class FilePath
    {
        #region ���� ���
        public const string UIResources = "UI/";
        public const string LobbyWorldPrefabPath = "Map/LobbyWorld/LobbySceneWorld";
        public const string LegendMenuUIPath = "UI/LegendMenu";
        public const string LegendMenuButtonPath = "UI/LegendMenuButton";
        public const string SettingUIPath = "UI/SettingPanel";
        public const string SettingButtonPath = "UI/SettingButton";
        public const string MatchingUIPath = "UI/MatchingPanel";
        public const string MatchingButtonPath = "UI/GameStartButton";
        public const string MapPath = "Map/LobbyWorld/";
        public const string UISpritePath = "UI/Sprite/";
        public const string CharacterDataTable = "Charater/CharacterData/CharacterDefaultStatusData";
        public const string CharacterPath = "Character/";
        public const string CharacterInGamePath = "_Ingame/";
        public const string CharacterInGamePrefabName = "_Ingame";
        #endregion
        public static string GetLobbyCharacterPath(CharacterType characterType)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string characterName = characterType.ToString();
            stringBuilder.Append(MapPath);
            stringBuilder.Append(characterName);
            return stringBuilder.ToString();
        }
        public static string GetCharacterSpritePath(CharacterType characterType)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string characterName = characterType.ToString();
            stringBuilder.Append(UISpritePath);
            stringBuilder.Append(characterName);
            return stringBuilder.ToString();
        }

        public static string GetChracterInGamePrefab(CharacterType characterType)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string characterName = characterType.ToString();
            stringBuilder.Append(CharacterPath);
            stringBuilder.Append(characterName);
            stringBuilder.Append($"/{characterName}{CharacterInGamePath}");
            stringBuilder.Append($"{characterName}{CharacterInGamePrefabName}");            
            return stringBuilder.ToString();
        }
    }
}
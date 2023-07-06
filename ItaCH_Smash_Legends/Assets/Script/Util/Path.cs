using System.Text;
using Util.Enum;

namespace Util.Path
{
    public static class FilePath
    {
        public static string GetLobbyCharacterPath(CharacterType characterType)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string characterName = characterType.ToString();
            stringBuilder.Append(ResourcesPath.MapPath);
            stringBuilder.Append(characterName);
            return stringBuilder.ToString();
        }
        public static string GetCharacterSpritePath(CharacterType characterType)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string characterName = characterType.ToString();
            stringBuilder.Append(ResourcesPath.UISpritePath);
            stringBuilder.Append(characterName);
            stringBuilder.Append($"/{characterName}");
            return stringBuilder.ToString();
        }

        public static string GetCharacterInGamePrefabPath(CharacterType characterType)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string characterName = characterType.ToString();
            stringBuilder.Append(ResourcesPath.CharacterPath);
            stringBuilder.Append(characterName);
            stringBuilder.Append($"/{characterName}{ResourcesPath.CharacterInGamePath}");
            stringBuilder.Append($"{characterName}{ResourcesPath.CharacterInGamePrefabName}");
            return stringBuilder.ToString();
        }
    }
}
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
            stringBuilder.Append(ResourcesManager.MapPath);
            stringBuilder.Append(characterName);
            return stringBuilder.ToString();
        }
        public static string GetCharacterSpritePath(CharacterType characterType)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string characterName = characterType.ToString();
            stringBuilder.Append(ResourcesManager.UISpritePath);
            stringBuilder.Append(characterName);
            return stringBuilder.ToString();
        }

        public static string GetCharacterInGamePrefabPath(CharacterType characterType)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string characterName = characterType.ToString();
            stringBuilder.Append(ResourcesManager.CharacterPath);
            stringBuilder.Append(characterName);
            stringBuilder.Append($"/{characterName}{ResourcesManager.CharacterInGamePath}");
            stringBuilder.Append($"{characterName}{ResourcesManager.CharacterInGamePrefabName}");
            return stringBuilder.ToString();
        }
    }
}
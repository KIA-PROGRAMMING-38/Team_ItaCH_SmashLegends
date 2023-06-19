using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Util
{
    #region 파일 경로
    public static class FilePath
    {
        public const string LegendMenuUIPath = "UI/LegendMenu";
        public const string LegendMenuButtonPath = "UI/LegendMenuButton";
        public const string SettingUIPath = "UI/SettingPanel";
        public const string SettingButtonPath = "UI/SettingButton";
        public const string MatchingUIPath = "UI/MatchingPanel";
        public const string MatchingButtonPath = "UI/GameStartButton";

        public static string GetLobbyCharacterPath(CharacterType characterType)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string characterName = characterType.ToString();
            stringBuilder.Append("Map/LobbyWorld/");
            stringBuilder.Append(characterName);
            Debug.Log(stringBuilder);
            return stringBuilder.ToString();
        }
    }

    #endregion
    public enum CharacterType
    { 
        Alice,
        Hook,
        Peter,
        NumOfCharacter
    }
}
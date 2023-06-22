using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Util.Enum;

namespace Util.Path
{
    public static class FilePath
    {
        #region ���� ���
        public const string LegendMenuUIPath = "UI/LegendMenu";
        public const string LegendMenuButtonPath = "UI/LegendMenuButton";
        public const string SettingUIPath = "UI/SettingPanel";
        public const string SettingButtonPath = "UI/SettingButton";
        public const string MatchingUIPath = "UI/MatchingPanel";
        public const string MatchingButtonPath = "UI/GameStartButton";
        public const string MapPath = "Map/LobbyWorld/";
        #endregion
        public static string GetLobbyCharacterPath(CharacterType characterType)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string characterName = characterType.ToString();
            stringBuilder.Append(MapPath);
            stringBuilder.Append(characterName);
            return stringBuilder.ToString();
        }
    }
}
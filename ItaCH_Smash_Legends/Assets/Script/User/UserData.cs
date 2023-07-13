using UnityEngine;
using Util.Enum;

public class UserData
{
    public string Name { get; set; }
    public int ID { get; set; }
    public TeamType Team { get; set; } // 결과창 UI 로직 변경 이후 삭제 필요
    public GameModeType SelectGameMode { get; set; }

    private CharacterType _selectedCharacter;
    public CharacterType SelectedCharacter
    {
        get
        {
            if (_selectedCharacter == CharacterType.None)
            {
                _selectedCharacter = (CharacterType)Random.Range((int)CharacterType.Alice, (int)CharacterType.MaxCount);
                return _selectedCharacter;
            }
            else
                return _selectedCharacter;
        }
        set => _selectedCharacter = value;
    }    
}
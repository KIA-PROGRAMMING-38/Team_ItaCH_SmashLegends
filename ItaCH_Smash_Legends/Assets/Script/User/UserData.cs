using UnityEngine;
using Util.Enum;

public class UserData
{
    public string Name { get; set; }
    public int Id { get; set; }
    public TeamType TeamType { get; set; }
    public GameModeType SelectGameMode { get; set; }

    private CharacterType _selectedCharacter;
    public CharacterType SelectedCharacter
    {
        get
        {
            if (_selectedCharacter == CharacterType.None)
            {
                _selectedCharacter = (CharacterType)Random.Range(0, 3);
                return _selectedCharacter;
            }
            else
                return _selectedCharacter;
        }
        set
        {
            _selectedCharacter = value;
        }
    }
    public void SetSelectedCharacter(CharacterType selectedCharacter)
    {
        _selectedCharacter = selectedCharacter;
    }
}
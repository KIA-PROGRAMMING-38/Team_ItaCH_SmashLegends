using Util.Enum;

public class UserData
{
    public string Name { get; set; }
    public int Id { get; set; }    
    public TeamType TeamType { get; set; }
    private CharacterType _selectedCharacter;      
    public CharacterType SelectedCharacter 
    {
        get
        {
            if (_selectedCharacter == default)
                return CharacterType.Peter;
            else 
                return _selectedCharacter; 
        }
        set => _selectedCharacter = value; 
    }
}
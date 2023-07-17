public static class StringLiteral
{
    #region SCENE NAME
    public const string LOBBY = "Lobby";
    public const string INGAME = "InGame";
    #endregion

    #region CONNECTION INFO TEXT
    public const string CONNECT_SERVER = "서버에 접속 중입니다.";
    public const string CONNECTION_SUCCESS = "서버 연결에 성공하였습니다.";
    public const string CONNECTION_FAILURE = "서버 연결에 실패하였습니다.";
    public const string ENTER_ROOM = "룸에 접속 중입니다.";
    public const string CREATE_ROOM = "게임이 없습니다. 새로 생성합니다.";
    public const string WAIT_PLAYER = "아레나가 열리고 있습니다. 상대를 기다리고 있습니다.";
    #endregion

    #region DATA PATH
    public const string LEGEND_STAT_DATA_PATH = "Assets/Resources/Data/LegendStatData.csv";
    #endregion

    #region LegendController 
    public static readonly string[] ACTIONS = new[] { "Move", "Jump", "DefaultAttack", "SmashAttack", "SkillAttack" };
    public static readonly string[] ANIMATION_CLIP = new[] { "Peter_FirstAttack", "Peter_SecondAttack" };
    public const string JUMP_ANIMATION_CLIP = "Peter_JumpAttack";
    #endregion

    #region Tag
    public const string HANGZONE = "HangZone";
    public const string GROUND = "Ground";
    public const string PLAYER = "Player";
    public const string DEFAULT_HIT = "DefaultHit";
    public const string HEAVY_HIT = "HeavyHit";
    #endregion
}

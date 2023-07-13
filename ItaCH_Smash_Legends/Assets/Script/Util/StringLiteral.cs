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

    #region FOLDER PATH
    public const string PREFAB_FOLDER = "Prefab";
    public const string SUFFIX_INGAME = "_Ingame";
    #endregion

    #region LegendController 
    public static readonly string[] Actions = new[] { "Move", "Jump", "DefaultAttack", "SmashAttack", "SkillAttack" };
    public static readonly string[] AnimationClip = new[] { "Peter_FirstAttack", "Peter_SecondAttack" };
    public const string JumpAnimationClip = "Peter_JumpAttack";
    #endregion

    #region Tag
    public const string HangZone = "HangZone";
    public const string Ground = "Ground";
    public const string Player = "Player";
    public const string DefaultHit = "DefaultHit";
    public const string HeavyHit = "HeavyHit";
    #endregion
}

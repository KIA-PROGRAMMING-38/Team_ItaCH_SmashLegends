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

    #region LegendController 
    public static readonly string[] ActionLiteral = new[] { "Run", "Jump", "DefaultAttack", "SmashAttack", "SkillAttack" };
    public static readonly string[] AnimationClipLiteral = new[] { "Peter_FirstAttack", "Peter_SecondAttack" };
    public const string JumpAnimationClipLiteral = "Peter_JumpAttack";
    public const string HangZone = "HangZone";
    public const string Ground = "Ground";
    #endregion
}

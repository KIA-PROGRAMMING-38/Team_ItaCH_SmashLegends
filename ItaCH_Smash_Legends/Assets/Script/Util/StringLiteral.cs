using System.Threading;

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
    public const string MAP_PREFAB_PATH = "Map/SingleLogBridge";
    public const string UI_FOLDER = "UI";
    public const string UI_SPRITE_FOLDER = "UI/Sprite";
    #endregion

    #region LegendController 
    public static readonly string[] ACTIONS = new[] { "Move", "Jump", "DefaultAttack", "SmashAttack", "SkillAttack" };
    public static readonly string[] ANIMATION_CLIP = new[] { "Peter_FirstAttack", "Peter_SecondAttack" };
    public const string JUMP_ANIMATION_CLIP = "Peter_JumpAttack";
    public const string ALICE_BOMB = "AliceBomb";
    #endregion

    #region Tag
    public const string HANGZONE = "HangZone";
    public const string GROUND = "Ground";
    public const string PLAYER = "Player";
    public const string DEFAULT_HIT = "DefaultHit";
    public const string HEAVY_HIT = "HeavyHit";
    #endregion

    #region INPUT ACTION MAP
    public const string PLAYER_INPUT = "PlayerInput";
    public const string FIRST_PLAYER_ACTIONS = "FirstPlayerActions";
    public const string SECOND_PLAYER_ACTIONS = "SecondPlayerActions";
    #endregion

    #region LAYER NAME
    public const string TEAM_BLUE = "TeamBlue";
    public const string TEAM_RED = "TeamRed";
    public const string HANG_ZONE = "HangZone";
    public const string HIT_ZONE = "HitZone";
    #endregion

    #region MAP
    public const string SPAWN_POINTS = "SpawnPoints";
    #endregion

    #region Legend SFX
    // Legend Common SFX
    public const string DEAFULTATTACK_ZERO = "DefaultAttack00";
    public const string DEFAULTATTACK_ONE = "DefaultAttack01";
    public const string DEFAULTATTACK_TWO = "DefaultAttack02";
    public const string DEFAULTATTACK_THREE = "DefaultAttack03";
    public const string DEFAULTATTACK_HIT = "DefaultAttack_Hit";
    public const string DOWN = "Down";
    public const string HANG = "Hang";
    public const string HEAVYATTACK = "HeavyAttack";
    public const string HEAVYATTACK_HIY = "HeavyAttack_Hit";
    public const string JUMP = "Jump";
    public const string JUMPATTACK = "JumpAttack";
    public const string JUMPLANDING = "JumpLanding";
    public const string ROLLBACK = "RollBack";
    public const string ROLLFRONT = "RollFront";
    public const string SKILLATTACK = "SkillAttack";
    public const string SKILLATTACK_HIT = "SkillAttack_Hit";
    public const string STEP_ZERO = "Step00";
    public const string STEP_ONE = "Step01";
    public const string STEP_TWO = "Step02";


    // Legend Individual SFX
    public const string SKILLATTACK_SHOT = "SkillAttack_Shot";
    public const string SKILLATTACK_START = "SkillAttack_Start";
    public const string SKILLATTACK_END = "SkillAttack_End";

    public const string MINE_ACTIVATE = "Mine_Activate";
    public const string MINE_EXPLODE = "Mine_Explode";
    public const string MINE_SET = "Mine_Set";
    #endregion

    #region Legend Voice
    #endregion
}
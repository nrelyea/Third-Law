using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : MonoBehaviour
{
    ///////////////////////////////////////////////////////////////////////////
    // CHANGE THIS WHENEVER NEW LEVELS ARE ADDED TO BUILD BEFORE PLAY LEVELS //
    public static int FirstLevelBuildIndex = 2;
    ///////////////////////////////////////////////////////////////////////////

    // controls if level should fade in on when level loading
    // is modified by scripts related to loading / reloading levels
    public static bool FadeInOnLoad = true;

    // Approximate height of player
    // used in scripts that need to have a general understanding of player height
    public static float PlayerHeight = 2.3f;

    // controls whether game is paused
    public static bool GameIsPaused = false;

    // controls whether player is allowed to use the Restart hotkey
    public static bool RestartAllowed = true;

    // these values hold KeyCodes for player input
    public static KeyCode PrimaryFireKey { get; set; }
    public static KeyCode SecondaryFireKey { get; set; }
    public static KeyCode MoveRightKey { get; set; }
    public static KeyCode MoveLeftKey { get; set; }
    public static KeyCode JumpKey { get; set; }
    public static KeyCode WalkKey { get; set; }
    public static KeyCode RestartKey { get; set; }

}

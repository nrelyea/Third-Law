using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : MonoBehaviour
{
    ///////////////////////////////////////////////////////////////////////////
    // CHANGE THIS WHENEVER NEW LEVELS ARE ADDED TO BUILD BEFORE PLAY LEVELS //
    public static int FirstLevelBuildIndex = 1;
    ///////////////////////////////////////////////////////////////////////////

    // controls if level should fade in on when level loading
    // is modified by scripts related to loading / reloading levels
    public static bool FadeInOnLoad = true;

    // Approximate height of player
    // used in scripts that need to have a general understanding of player height
    public static float PlayerHeight = 2.3f;
    
}

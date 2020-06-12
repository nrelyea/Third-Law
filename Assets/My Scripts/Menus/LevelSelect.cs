using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    ///////////////////////////////////////////////////////////////////////////
    // CHANGE THIS WHENEVER NEW LEVELS ARE ADDED TO BUILD BEFORE PLAY LEVELS //
    public static int FirstLevelBuildIndex = 1;
    ///////////////////////////////////////////////////////////////////////////

    void Start()
    {
        if (!PlayerPrefs.HasKey("mostRecentLevelUnlocked"))
        {
            PlayerPrefs.SetInt("mostRecentLevelUnlocked", FirstLevelBuildIndex);
            PlayerPrefs.Save();
        }
    }
}

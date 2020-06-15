using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    void Start()
    {
        if (!PlayerPrefs.HasKey("mostRecentLevelUnlocked"))
        {
            PlayerPrefs.SetInt("mostRecentLevelUnlocked", GlobalVars.FirstLevelBuildIndex);
            PlayerPrefs.Save();
        }
    }
}

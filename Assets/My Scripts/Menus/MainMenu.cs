using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    void Start()
    {
        if (!PlayerPrefs.HasKey("mostRecentLevelUnlocked"))
        {
            PlayerPrefs.SetInt("mostRecentLevelUnlocked", GlobalVars.FirstLevelBuildIndex);
            PlayerPrefs.Save();
        }
    }
    
    public void LoadLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }
}

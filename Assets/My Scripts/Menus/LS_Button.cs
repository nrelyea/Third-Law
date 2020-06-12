using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LS_Button : MonoBehaviour
{
    public int LevelNumber;

    // Start is called before the first frame update
    void Start()
    {
        int mostRecentLevelUnlocked = PlayerPrefs.GetInt("mostRecentLevelUnlocked");
        if (LevelSelect.FirstLevelBuildIndex - 1 + LevelNumber > mostRecentLevelUnlocked)
        {
            GetComponent<Button>().interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HandleClick()
    {
        SceneManager.LoadScene(LevelSelect.FirstLevelBuildIndex - 1 + LevelNumber);
    }
}

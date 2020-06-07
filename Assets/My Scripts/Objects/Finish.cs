using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public int TargetLevelNumber;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D objInfo)
    {
        if(objInfo.name == "Player")
        {
            Debug.Log("LEVEL COMPLETE!");

            SceneManager.LoadScene(LevelSelect.FirstLevelBuildIndex - 1 + TargetLevelNumber);

            if (TargetLevelNumber > PlayerPrefs.GetInt("mostRecentLevelUnlocked"))
            {
                PlayerPrefs.SetInt("mostRecentLevelUnlocked", TargetLevelNumber);
            }
        }
    }
}

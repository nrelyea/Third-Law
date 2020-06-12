using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Reflection;

public class Finish : MonoBehaviour
{
    public int CurrentLevelNumber;
    public int TargetLevelNumber;

    public GameObject timer;

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
            ClearConsole();
            Debug.Log("LEVEL COMPLETE!");

            // save time completed and see if it is a new PB
            ScoreManagement sm = new ScoreManagement();
            if(sm.SaveTime(CurrentLevelNumber, timer.GetComponent<UI>().GetTime()))
            {
                Debug.Log("New Personal Best!");
            }

            // display time
            try
            {
                Debug.Log("Time: " + timer.GetComponent<UI>().FormatTime());
            }
            catch
            {
                Debug.Log("ERROR: Finish not passed proper TimeManager object needed to report time to complete level");
            }

            SceneManager.LoadScene(LevelSelect.FirstLevelBuildIndex - 1 + TargetLevelNumber);

            if (TargetLevelNumber > PlayerPrefs.GetInt("mostRecentLevelUnlocked"))
            {
                PlayerPrefs.SetInt("mostRecentLevelUnlocked", TargetLevelNumber);
                PlayerPrefs.Save();
            }
        }
    }

    private void ClearConsole()
    {
        var assembly = Assembly.GetAssembly(typeof(SceneView));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
}

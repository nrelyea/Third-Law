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
            GameObject logisticsObject = GameObject.Find("Logistics Manager");
            if(sm.SaveTime(CurrentLevelNumber, logisticsObject.GetComponent<UI>().GetTime()))
            {
                Debug.Log("New Personal Best!");
            }

            // display time
            try
            {
                Debug.Log("Time: " + logisticsObject.GetComponent<UI>().FormatTime());
            }
            catch
            {
                Debug.Log("ERROR: Finish not passed proper TimeManager object needed to report time to complete level");
            }

            int TargetIndex = GlobalVars.FirstLevelBuildIndex - 1 + TargetLevelNumber;
            //Debug.Log("Target index: " + TargetIndex + "  (" + GlobalVars.FirstLevelBuildIndex + " - 1 + " + TargetLevelNumber + ")");
            if (TargetIndex > PlayerPrefs.GetInt("mostRecentLevelUnlocked"))
            {
                PlayerPrefs.SetInt("mostRecentLevelUnlocked", TargetIndex);
                PlayerPrefs.Save();
            }

            // Fade and Load to next level using the LevelFading script
            Transform image = GameObject.Find("UI Canvas").transform.Find("FadeImage");
            LevelFading fadeScript = image.gameObject.GetComponent<LevelFading>();
            fadeScript.FadeToLevel(TargetIndex);
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

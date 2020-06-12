using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManagement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // saves time and returns true if time being saved is new PB
    public bool SaveTime(int levelNumber, float time)
    {
        float currentPB = PlayerPrefs.GetFloat("Level" + levelNumber + "PB");
        if (time < currentPB || currentPB == 0)
        {
            PlayerPrefs.SetFloat("Level" + levelNumber + "PB", time);
            PlayerPrefs.SetString("Level" + levelNumber + "PB_Formatted", FormatTime(time));
            return true;
        }
        return false;
    }

    private string FormatTime(float inputSeconds)
    {
        int minutes = 0;

        while (inputSeconds > 60)
        {
            minutes++;
            inputSeconds -= 60;
        }

        string outputMinutes;
        if (minutes < 10)
        {
            outputMinutes = "0" + minutes.ToString();
        }
        else
        {
            outputMinutes = minutes.ToString();
        }

        string outputSeconds;
        try
        {
            if (inputSeconds < 10)
            {
                outputSeconds = "0" + (inputSeconds.ToString()).Substring(0, 5);
            }
            else
            {
                outputSeconds = (inputSeconds.ToString()).Substring(0, 6);
            }
        }
        catch
        {
            outputSeconds = "00.000";
        }

        return outputMinutes + ":" + outputSeconds;
    }
}

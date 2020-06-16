using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public static UI instance;
    public TextMeshProUGUI TMP;

    private float timeSeconds;

    // Start is called before the first frame update
    void Start()
    {      

        if (instance == null)
        {
            instance = this;
        }

        timeSeconds = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeSeconds += Time.deltaTime;
        //Debug.Log(time);
        TMP.text = FormatTime();
    }

    public float GetTime()
    {
        return timeSeconds;
    }

    public string FormatTime()
    {
        float inputSeconds = timeSeconds;

        int minutes = 0;

        while(inputSeconds > 60)
        {
            minutes++;
            inputSeconds -= 60;
        }

        string outputMinutes;
        if(minutes < 10)
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

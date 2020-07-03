using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OtherInputs : MonoBehaviour
{
    
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {        
        if(GlobalVars.RestartAllowed && Input.GetKey(GlobalVars.RestartKey))
        {
            if (GlobalVars.GameIsPaused)
            {
                // resume game if necessary
                GameObject canvas = GameObject.Find("UI Canvas");
                Transform panel = canvas.transform.Find("Pause Panel");
                panel.gameObject.SetActive(false);
                Time.timeScale = 1f;
                GlobalVars.GameIsPaused = false;
            }
            
            // Specify that on the next load, the scene won't fade in (as this is just a level reset)
            GlobalVars.FadeInOnLoad = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);            
        }
    }
}

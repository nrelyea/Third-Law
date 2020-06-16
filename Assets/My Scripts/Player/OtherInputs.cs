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
        if (Input.GetButtonDown("Menu"))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetButtonDown("Restart"))
        {
            // Specify that on the next load, the scene won't fade in (as this is just a level reset)
            GlobalVars.FadeInOnLoad = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);            
        }
    }
}

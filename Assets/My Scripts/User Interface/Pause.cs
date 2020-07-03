using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;



public class Pause : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject ControlsPanel;

    public TextMeshProUGUI RestartButtonText;


    // Start is called before the first frame update
    void Start()
    {
        PausePanel.SetActive(false);
        ControlsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            if (GlobalVars.GameIsPaused)
            {
                if (ControlsPanel.activeSelf)   // exiting out of Controls menu to Pause menu
                {
                    // reset / refresh controls panel in case it was exited from while setting new controls
                    gameObject.GetComponent<ControlsManager>().ResetControlsPanel();

                    UpdateRestartButtonText();

                    GlobalVars.RestartAllowed = true;

                    PausePanel.SetActive(true);
                    ControlsPanel.SetActive(false);
                }
                else
                {
                    ResumeGame();
                }
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
        GlobalVars.GameIsPaused = false;
    }

    public void PauseGame()
    {
        UpdateRestartButtonText();
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
        GlobalVars.GameIsPaused = true;
    }

    public void RestartLevel()
    {
        ResumeGame();

        // Restart Level
        GlobalVars.FadeInOnLoad = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadOptions()
    {
        GlobalVars.RestartAllowed = false;
        ControlsPanel.SetActive(true);
        PausePanel.SetActive(false);
    }

    public void LoadMainMenu()
    {
        ResumeGame();
        SceneManager.LoadScene("Main Menu");
    }

    public void UpdateRestartButtonText()
    {
        string formattedControl = gameObject.GetComponent<ControlsManager>().FormattedControlName(PlayerPrefs.GetString("restartKey"));
        RestartButtonText.text = "Restart (" + formattedControl + ")";
    }









    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


// This script is attached to the UI Canvas
public class ControlsManager : MonoBehaviour
{
    private Transform ControlsPanelTransform;

    private bool WaitingForKey = false;
    private string ActionChanging;

    public Color DefaultButtonTextColor;
    public Color WarningButtonTextColor;

    private HashSet<string> usedKeys = new HashSet<string> { };
    private HashSet<string> duplicateKeys = new HashSet<string> { };

    // handling BUG where assigning a Key to Mouse0 while mouse is over the button will make it be clicked right after assignment
    private bool LeftMouseDown = false; // keeping track of whether left mouse is down to avoid the bug
    
    void Awake()
    {
        // Assign default keys for controls if they have not been assigned before
        if (!PlayerPrefs.HasKey("primaryKey"))
        {
            PlayerPrefs.SetString("primaryKey", "Mouse0");
            PlayerPrefs.SetString("secondaryKey", "Mouse1");
            PlayerPrefs.SetString("rightKey", "D");
            PlayerPrefs.SetString("leftKey", "A");
            PlayerPrefs.SetString("jumpKey", "W");
            PlayerPrefs.SetString("walkKey", "S");
            PlayerPrefs.SetString("restartKey", "R");
        }

        UpdateGlobalKeyCodes();
    }

    // Start is called before the first frame update
    void Start()
    {
        ControlsPanelTransform = gameObject.transform.Find("Controls Panel");

        ResetControlsPanel();
    }

    // Update is called once per frame
    void Update()
    {
        // detects mouse presses being submitted as new controls, as OnGUI() only triggered by keyboard events
        if (WaitingForKey)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SubmitNewKey(KeyCode.Mouse0);
                LeftMouseDown = true;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                SubmitNewKey(KeyCode.Mouse1);
            }
            else if (Input.GetMouseButtonDown(2))
            {
                SubmitNewKey(KeyCode.Mouse2);
            }           
        }

        if (Input.GetMouseButtonUp(0))
        {
            LeftMouseDown = false;
        }
    }

    // called when keyboard input is given
    void OnGUI()
    {
        if (WaitingForKey)
        {
            Event e = Event.current;
            if (e.isKey && e.keyCode.ToString() != "Escape")
            {
                SubmitNewKey(e.keyCode);
            }
        }
    }

    // saves new keyboard or mouse click as the new control for the action stored by ActionChanging
    private void SubmitNewKey(KeyCode key)
    {        
        switch (ActionChanging)
        {
            case "Primary Fire":
                PlayerPrefs.SetString("primaryKey", key.ToString());
                break;
            case "Secondary Fire":
                PlayerPrefs.SetString("secondaryKey", key.ToString());
                break;
            case "Move Right":
                PlayerPrefs.SetString("rightKey", key.ToString());
                break;
            case "Move Left":
                PlayerPrefs.SetString("leftKey", key.ToString());
                break;
            case "Jump":
                PlayerPrefs.SetString("jumpKey", key.ToString());
                break;
            case "Walk":
                PlayerPrefs.SetString("walkKey", key.ToString());
                break;
            case "Restart Level":
                PlayerPrefs.SetString("restartKey", key.ToString());
                break;
        }

        UpdateGlobalKeyCodes();

        ResetControlsPanel();
    }

    // Event called by buttons, with the parameter referring to the action (written to left of button) to be changed
    public void ButtonClicked(string action)
    {
        if (LeftMouseDown) return;  // part of handling of Mouse0 assignment bug mentioned above
        
        ActionChanging = action;

        foreach (Transform child in ControlsPanelTransform)
        {
            if(child.name == action)
            {
                child.Find("Button").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
            }
            else
            {
                try
                {
                    child.Find("Button").GetComponent<Button>().interactable = false;
                }
                catch { };
            }
        }

        WaitingForKey = true;
    }

    // Resets and refreshes all elements in the Controls Panel
    public void ResetControlsPanel()
    {
        WaitingForKey = false;

        // re-enable all buttons
        foreach (Transform child in ControlsPanelTransform)
        {
            try
            {
                child.Find("Button").GetComponent<Button>().interactable = true;
            }
            catch { };
        }     

        RefreshButtonLabels();
    }

    // refreshes button labels to reflect the current keycode that controls the appropriate action
    private void RefreshButtonLabels()
    {
        usedKeys.Clear();
        duplicateKeys.Clear();

        // Surrounded with try / catch because one random null exception error happens
        // but everything that needs to be iterated through is successful soooooo idk whatever lol
        try
        {
            // Go through each of the children of the panel and do different things based on the object denoting a certain action / control
            foreach (Transform child in ControlsPanelTransform)
            {
                // If child has a button object, set the Button's text to the default color
                if (child.Find("Button") != null) child.Find("Button").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().color = DefaultButtonTextColor;

                // for each action, change the text component of the TMP of the Button object to reflect the current key used to control that action
                switch (child.name)
                {
                    case "Primary Fire":
                        UpdateButtonTextAndAssessDuplicate(child, "primaryKey");
                        break;
                    case "Secondary Fire":
                        UpdateButtonTextAndAssessDuplicate(child, "secondaryKey");                        
                        break;
                    case "Move Right":
                        UpdateButtonTextAndAssessDuplicate(child, "rightKey");                        
                        break;
                    case "Move Left":
                        UpdateButtonTextAndAssessDuplicate(child, "leftKey");                        
                        break;
                    case "Jump":
                        UpdateButtonTextAndAssessDuplicate(child, "jumpKey");                        
                        break;
                    case "Walk":
                        UpdateButtonTextAndAssessDuplicate(child, "walkKey");                        
                        break;
                    case "Restart Level":
                        UpdateButtonTextAndAssessDuplicate(child, "restartKey");                        
                        break;
                }
            }
        }
        catch { }

        // At this point, the duplicateKeys set holds any controls that have been assigned to more than one action, 
        // so the next step is to indicate this by changing the colors of the text on their associated buttons

        foreach (Transform child in ControlsPanelTransform)
        {
            // if the object has a button child and the current text of that button exists in the duplicates set...
            if(child.Find("Button") != null && duplicateKeys.Contains(child.Find("Button").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text))
            {
                // change this Button's to the color given by WarningButtonTextColor
                child.Find("Button").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().color = WarningButtonTextColor;
            }
        }
    }

    // Helper for RefreshButtonLabels
    // Performs necessary actions to update button text and determine if this button's control KeyCode is being used for another action as well (duplicate)
    private void UpdateButtonTextAndAssessDuplicate(Transform child, string pref)
    {
        // first, assign text for this button to reflect the PlayerPref storing the control for this action
        child.Find("Button").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = FormattedControlName(PlayerPrefs.GetString(pref));
        // second, determine if this control has already been added to the "usedKeys" set, and if so, add the formatted version of it to the "duplicateKeys" set
        if (usedKeys.Contains(PlayerPrefs.GetString(pref))) duplicateKeys.Add(FormattedControlName(PlayerPrefs.GetString(pref)));
        // otherwise, add it to the "usedKeys" set
        else usedKeys.Add(PlayerPrefs.GetString(pref));
    }

    // returns formatted names for control inputs to provide more clarity
    public string FormattedControlName(string control)
    {
        switch (control)
        {
            case "Mouse0": return "Left Mouse";         // changes certain inputs names to be more clear as to what they refer to
            case "Mouse1": return "Right Mouse";
            case "Mouse2": return "Middle Mouse";
            default:
                for(int i = 1; i < control.Length; i++)     // Adds spaces as needed to make controls more readable (ie 'LeftArrow' becomes 'Left Arrow')
                {
                    if(!Char.IsUpper(control[i-1]) && Char.IsUpper(control[i]))
                    {
                        control = control.Insert(i, " ");
                        i++;
                    }
                }
                return control;
        }
    }

    // Update GlobalVars keycode values as needed to reflect current PlayerPref stored controls
    private void UpdateGlobalKeyCodes()
    {
        GlobalVars.PrimaryFireKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("primaryKey", "Mouse0"));
        GlobalVars.SecondaryFireKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("secondaryKey", "Mouse1"));
        GlobalVars.MoveRightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rightKey", "D"));
        GlobalVars.MoveLeftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("leftKey", "A"));
        GlobalVars.JumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("jumpKey", "W"));
        GlobalVars.WalkKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("walkKey", "S"));
        GlobalVars.RestartKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("restartKey", "R"));
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    //public int TargetID;
    public string TargetName;

    public Sprite ActiveSprite;
    public Sprite InActiveSprite;

    public bool Activated = false;

    private SpriteRenderer spriteRenderer;

    private HashSet<string> RegularActivators;
    private HashSet<string> FrozenActivators;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == null || !Activated)
        {
            SetSprite(InActiveSprite);
        }
        else
        {
            SetSprite(ActiveSprite);
        }

        RegularActivators = new HashSet<string> { };
        FrozenActivators = new HashSet<string> { };

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D objInfo)
    {
        if (!IsBullet(objInfo))
        {
            IO_Collision io = objInfo.GetComponent<IO_Collision>();
            if (io != null && FrozenActivators.Contains(objInfo.name))
            {
                FrozenActivators.Remove(objInfo.name);
            }
            RegularActivators.Add(objInfo.name);

            // Update button and target to reflect changes based on total activators
            UpdateButtonAndTarget(RegularActivators.Count + FrozenActivators.Count > 0);
        }
    }

    void OnTriggerStay2D(Collider2D objInfo)
    {
        //ActivateButton(objInfo);
    }

    void OnTriggerExit2D(Collider2D objInfo)
    {
        if (!IsBullet(objInfo))
        {
            IO_Collision io = objInfo.GetComponent<IO_Collision>();
            if(io != null && io.Frozen)
            {
                FrozenActivators.Add(objInfo.name);
            }
            RegularActivators.Remove(objInfo.name);

            // Update button and target to reflect changes based on total activators
            UpdateButtonAndTarget(RegularActivators.Count + FrozenActivators.Count > 0);

        }
    }

    // FOR DEBUGGING, NOT CALLED ANYWHERE RIGHT NOW
    void PrintSets()
    {
        string reg = "";
        foreach(string str in RegularActivators)
        {
            reg += str + ", ";
        }

        string fro = "";
        foreach (string str in FrozenActivators)
        {
            fro += str + ", ";
        }
        Debug.Log("Regular activators: " + reg + "  |  Frozen activators: " + fro);
    }

    private void UpdateButtonAndTarget(bool signal)
    {
        if(Activated != signal)
        {
            if (signal)
            {
                //Debug.Log("Activating!");
                Activated = true;
                SetSprite(ActiveSprite);
            }
            else
            {
                //Debug.Log("Deactivating!");
                Activated = false;
                SetSprite(InActiveSprite);
            }

            try
            {
                Door doorScript = GameObject.Find(TargetName).GetComponent<Door>();
                doorScript.RecieveSignal(signal);
            }
            catch (Exception e)
            {
                Debug.Log("ERROR: Button '" + gameObject.name + "' has invalid target '" + TargetName + "'\n\n");
            }
        }     
    }

    private void SetSprite(Sprite s)
    {
        spriteRenderer.sprite = s;
    }

    private bool IsBullet(Collider2D obj)
    {
        return (obj.name == "PrimaryBullet(Clone)" || obj.name == "SecondaryBullet(Clone)");
    }
}

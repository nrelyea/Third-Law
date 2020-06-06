using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    //public int TargetID;
    public string TargetName;

    public Sprite ActiveSprite;
    public Sprite InActiveSprite;

    public bool Activated = false;

    private SpriteRenderer spriteRenderer;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D objInfo)
    {
        ActivateButton(objInfo);
    }

    void OnTriggerStay2D(Collider2D objInfo)
    {
        ActivateButton(objInfo);
    }

    void OnTriggerExit2D(Collider2D objInfo)
    {
        DeactivateButton(objInfo);
    }

    void ActivateButton(Collider2D objInfo)
    {
        if (!Activated && !IsBullet(objInfo))
        {
            Activated = true;
            SetSprite(ActiveSprite);
            UpdateTarget(true);
        }
    }

    void DeactivateButton(Collider2D objInfo)
    {
        if (!IsBullet(objInfo))
        {
            IO_Collision io = objInfo.GetComponent<IO_Collision>();
            // only deactivate button if either a non-IO deactivated it or the IO that deactivated didn't actually leave but is just frozen
            if (io == null || (io != null && !io.Frozen))
            {
                Activated = false;
                SetSprite(InActiveSprite);
                UpdateTarget(false);
            }
        }
    }  

    private void UpdateTarget(bool signal)
    {
        try
        {
            Door doorScript = GameObject.Find(TargetName).GetComponent<Door>();
            //Debug.Log("Door open: " + doorScript.DoorOpen);
            doorScript.RecieveSignal(signal);
        }
        catch (Exception e)
        {
            Debug.Log("ERROR: Button '" + gameObject.name + "' has invalid target '" + TargetName + "'\n\n");
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

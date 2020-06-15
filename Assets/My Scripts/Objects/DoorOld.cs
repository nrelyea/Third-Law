using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOld : MonoBehaviour
{
    public int ButtonsNeeded;           // number of buttons being pressed / signals needed to be recieved to open door
    private int SignalsRecieved = 0;    // current number of signals recieved


    public Sprite OpenSprite;
    public Sprite ClosedSprite;

    private bool DoorOpen = false;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == null || !DoorOpen)
        {
            SetSprite(ClosedSprite);
        }
        else
        {
            SetSprite(OpenSprite);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void RecieveSignal(bool signal)
    {
        if (signal)
        {
            SignalsRecieved++;
        }
        else if(!signal && SignalsRecieved > 0)
        {
            SignalsRecieved--;
        }

        if (SignalsRecieved >= ButtonsNeeded)
        {
            DoorOpen = true;
            SetSprite(OpenSprite);
        }
        else
        {
            DoorOpen = false;
            SetSprite(ClosedSprite);
        }
    }

    private void SetSprite(Sprite s)
    {
        spriteRenderer.sprite = s;
    }


}

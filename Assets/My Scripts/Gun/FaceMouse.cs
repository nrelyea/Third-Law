using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMouse : MonoBehaviour
{

    public int offsetAngle;
    private float prevRelativeX;   // stores previously calculated relative x position to be compared to current


    // Start is called before the first frame update
    void Start()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        float currentRelativeX = mousePosition.x - transform.position.x;

        if(currentRelativeX < 0)
        {
            Flip();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ignore all gun movement handling if game is paused
        if (GlobalVars.GameIsPaused) return;

        faceMouse();
    }

    void faceMouse()
    {
        // Rotate gun to aim at mouse

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        float currentRelativeX = mousePosition.x - transform.position.x;
        float currentRelativeY = mousePosition.y - transform.position.y;

        // HANDLING BUG: If x < 0 and y ~= 0, it flips incorrectly. This should handle that bug
        //float old = currentRelativeY;
        if (currentRelativeX < 0 && Math.Abs((double)currentRelativeY) < 0.05)
        {
            if(currentRelativeY > 0.0f)
            {
                //currentRelativeY += 0.05f;
                currentRelativeY += (0.04f + (currentRelativeX / -1000));
            }
            else
            {
                //currentRelativeY -= 0.05f;
                currentRelativeY -= (0.04f + (currentRelativeX / -1000));
            }
        }
        //Debug.Log("X: " + currentRelativeX + "   old: " + old + "  new: " + currentRelativeY);

        Vector2 direction = new Vector2(currentRelativeX, currentRelativeY);

        direction = Quaternion.AngleAxis(offsetAngle, Vector3.forward) * direction;

        transform.up = direction;
        
        // Flip mouse if necessary to reflect mouse being on other side of player

        if (prevRelativeX * currentRelativeX < 0)    // if mouse has passed over the center line of the body
        {
            Flip();
        }

        prevRelativeX = currentRelativeX;

    }

    public void Flip()
    {
        // Multiply the gun's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.y *= -1;
        transform.localScale = theScale;
    }
}

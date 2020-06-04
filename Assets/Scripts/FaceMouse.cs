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
        faceMouse();
    }

    void faceMouse()
    {
        // Rotate gun to aim at mouse

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        float currentRelativeX = mousePosition.x - transform.position.x;
        float currentRelativeY = mousePosition.y - transform.position.y;

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

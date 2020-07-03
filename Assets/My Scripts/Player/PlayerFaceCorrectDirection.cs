using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFaceCorrectDirection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ignore all animation flipping if game is paused
        if (GlobalVars.GameIsPaused) return;

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        float currentRelativeX = mousePosition.x - transform.position.x;

        if (mousePosition.x - transform.position.x > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

        //Debug.Log("relative x: " + currentRelativeX);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePointAdjustment : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Flip();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Flip()
    {
        // Multiply the fire points's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.y *= -1;
        transform.localScale = theScale;
    }
}

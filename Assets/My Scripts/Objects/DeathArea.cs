using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DeathArea : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (!IsBullet(collisionInfo))
        {
            if(collisionInfo.gameObject.name == "Player")
            {
                // Call for player to be killed with animation type '0'
                collisionInfo.gameObject.GetComponent<PlayerStatus>().KillPlayer(0);               
            }
            else
            {
                IO_Collision io = collisionInfo.gameObject.GetComponent<IO_Collision>();
                if(io != null)
                {
                    // Reset the object's position, rotation, and velocities to its initial ones
                    Rigidbody2D rb = collisionInfo.gameObject.GetComponent<Rigidbody2D>();
                    rb.velocity = new Vector2(0, 0);
                    rb.angularVelocity = 0;

                    collisionInfo.gameObject.transform.position = io.GetInitialPosition();
                    collisionInfo.gameObject.transform.rotation = io.GetInitialRotation();

                    //Destroy(collisionInfo.gameObject);
                }
            }           
        }
    }    

    private bool IsBullet(Collision2D col)
    {
        return (col.gameObject.name == "PrimaryBullet(Clone)" || col.gameObject.name == "SecondaryBullet(Clone)");
    }
}

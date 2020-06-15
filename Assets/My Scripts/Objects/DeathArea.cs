using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DeathArea : MonoBehaviour
{
    private GameObject Hole;
    private bool HoleShrinking;
    private bool HoleShrinkComplete;

    // Start is called before the first frame update
    void Start()
    {
        Hole = GameObject.Find("DeathHole");
        HoleShrinking = false;
    }

    void FixedUpdate()
    {
        // control shrinking of death hole effect
        if (HoleShrinking)
        {
            if (Hole.transform.localScale.x > 1)
            {
                Hole.transform.localScale = Vector3.Scale(Hole.transform.localScale, new Vector3(0.97f, 0.97f, 0.97f));
                HoleShrinkComplete = true;
            }
            else if (HoleShrinkComplete)
            {
                HoleShrinking = false;

                Transform image = GameObject.Find("UI Canvas").transform.Find("FadeImage");
                LevelFading fadeScript = image.gameObject.GetComponent<LevelFading>();
                fadeScript.FadeToLevel(SceneManager.GetActiveScene().buildIndex);                
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (!IsBullet(collisionInfo))
        {
            if(collisionInfo.gameObject.name == "Player")
            {
                // force zoom target to be certain number
                CameraZoom zoomScript = GameObject.Find("Zoom").GetComponent<CameraZoom>();
                zoomScript.ForceZoom(7);

                // stop player from moving
                Rigidbody2D playerRB = collisionInfo.gameObject.GetComponent<Rigidbody2D>();
                playerRB.bodyType = RigidbodyType2D.Static;

                // generate effect of circular spotlight focusing on where player died
                DeathHoleEffect(collisionInfo.gameObject.transform.position, 1);

                ////////////////////////////////////////////////
                // MAKE PLAYER DYING ANIMATION / EFFECTS HERE //
                ////////////////////////////////////////////////

                // remove the gun
                Transform gunTransform = collisionInfo.gameObject.transform.Find("Gun");
                Destroy(gunTransform.gameObject);


                //Destroy(collisionInfo.gameObject);
            }
            else
            {
                IO_Collision io = collisionInfo.gameObject.GetComponent<IO_Collision>();
                if(io != null)
                {
                    Debug.Log("sup IO");

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

    private void DeathHoleEffect(Vector3 targetPos, float yOffset)
    {
        Hole.transform.localScale = new Vector3(35, 35, 1);
        Hole.transform.position = new Vector3(targetPos.x, targetPos.y + yOffset, 0);
        HoleShrinking = true;
    }

    private bool IsBullet(Collision2D col)
    {
        return (col.gameObject.name == "PrimaryBullet(Clone)" || col.gameObject.name == "SecondaryBullet(Clone)");
    }
}
